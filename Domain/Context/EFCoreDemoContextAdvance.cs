using EFCoreDemo.Domain.Entities.AdvanceEntities;
using EFCoreDemo.Domain.Entities.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCoreDemo.Domain.Context;

public class EFCoreDemoContextAdvance : DbContext
{
    public EFCoreDemoContextAdvance(DbContextOptions<EFCoreDemoContextAdvance> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(a =>
        {
            a.ToTable(nameof(Author));
            a.HasKey(a => a.Id);
            a.Property(x => x.AuthorName).IsRequired().HasMaxLength(256);
            a.HasMany(x => x.Books).WithMany(x => x.Authors);

            //a.HasMany(x => x.Books).WithMany(x => x.Authors)
            //.UsingEntity<Dictionary<string, object>>(
            //    "AuthorBook", // Name of the join table
            //    j => j.HasOne<Book>().WithMany().HasForeignKey("BookId"),
            //    j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"));
        });

        modelBuilder.Entity<Book>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.BookName).IsRequired().HasMaxLength(256);
            b.OwnsMany(x => x.Reviews, r =>
            {
                r.ToJson();
            });
        });
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCreationAuditProperties(entry.Entity, "Admin");
                    break;

                case EntityState.Modified:
                    SetModificationAuditProperties(entry.Entity, "Admin");
                    break;

                case EntityState.Deleted:
                    CancelDeletionForSoftDelete(entry);
                    SetModificationAuditProperties(entry.Entity, "Admin");
                    break;

                default:
                    SetCreationAuditProperties(entry.Entity, "Admin");
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    private static void SetCreationAuditProperties(object entityAsObj, string userName)
    {
        var entityWithCreationTime = entityAsObj as IHasCreationTime;
        if (entityWithCreationTime == null)
        {
            return;
        }

        if (entityWithCreationTime.CreatedAt == default)
        {
            entityWithCreationTime.CreatedAt = DateTime.Now;
        }

        if (entityAsObj is not ICreationAudited)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(userName))
        {
            return;
        }

        var entity = entityAsObj as ICreationAudited;
        if (entity!.CreatedBy != null)
        {
            return;
        }
        entity!.CreatedBy = userName;
    }

    private static void SetModificationAuditProperties(object entityAsObj, string userName)
    {
        if (entityAsObj is IHasModificationTime)
        {
            entityAsObj.As<IHasModificationTime>().ModifiedAt = DateTime.Now;
        }

        if (entityAsObj is not IModificationAudited)
        {
            return;
        }

        var entity = entityAsObj.As<IModificationAudited>();

        if (userName == null)
        {
            entity.ModifiedBy = null;
            return;
        }
        entity.ModifiedBy = userName;
    }

    private static void CancelDeletionForSoftDelete(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDelete)
        {
            return;
        }

        entry.Reload();
        entry.State = EntityState.Modified;
        entry.Entity.As<ISoftDelete>().IsDeleted = true;
    }
}