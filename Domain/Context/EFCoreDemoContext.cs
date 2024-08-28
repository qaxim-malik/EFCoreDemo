using EFCoreDemo.Domain.Entities.SimpleEntities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDemo.Domain.Context;

public class EFCoreDemoContext : DbContext
{
    public EFCoreDemoContext(DbContextOptions<EFCoreDemoContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Product> Products { get; set; }
}