using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreDemo.Domain.Context;
using EFCoreDemo.Domain.Entities.AdvanceEntities;
using EFCoreDemo.Domain.Entities.SimpleEntities;
using EFCoreDemo.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class EFCoreDemoController : ControllerBase
{
    private readonly EFCoreDemoContext _dbContext;
    private readonly EFCoreDemoContextAdvance _dbContextAdvance;
    private readonly IMapper _mapper;

    public EFCoreDemoController(EFCoreDemoContext dbContext,
                                IMapper mapper,
                                EFCoreDemoContextAdvance dbContextAdvance)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dbContextAdvance = dbContextAdvance;
    }

    [HttpPost(nameof(CreateTeacher))]
    public async Task<ActionResult<Guid>> CreateTeacher(AddTeacherDto teacher)
    {
        var teacherEntity = new Teacher
        {
            TeacherName = teacher.TeacherName,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Admin"
        };
        await _dbContext.Teacher.AddAsync(teacherEntity);
        var result = await _dbContext.SaveChangesAsync();
        if (result > 0)
        {
            return Ok(teacherEntity.Id);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost(nameof(CreateStudent))]
    public async Task<ActionResult<int>> CreateStudent(AddStudentDto student)
    {
        var studentEntity = new Student
        {
            StudentName = student.StudentName,
            Email = student.Email,
            TeacherId = student.TeacherId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Admin"
        };
        await _dbContext.Student.AddAsync(studentEntity);
        var result = await _dbContext.SaveChangesAsync();
        if (result > 0)
        {
            return Ok(studentEntity.Id);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost(nameof(GetAllTeacherWithStudents))]
    public async Task<ActionResult<IEnumerable<GetTeacherWithStudentsDto>>> GetAllTeacherWithStudents()
    {
        //int pageNumber = 0;
        //int pageSize = 10;

        //int skip = pageSize * pageNumber;
        //int take = pageSize;

        //var teachersFromDb = await _dbContext.Teacher.AsNoTracking()
        //                                             .Include(x => x.Students)
        //                                             .Skip(skip)
        //                                             .Take(take)
        //                                             .ToListAsync();

        var teachersFromDb = await _dbContext.Teacher.AsNoTracking()
                                                     .Include(x => x.Students)
                                                     .ToListAsync();

        var teacherWithStudents = new List<GetTeacherWithStudentsDto>();

        foreach (var teacher in teachersFromDb)
        {
            var _studentsList = new List<GetStudentsDto>();
            if (teacher.Students is not null)
            {
                foreach (var student in teacher.Students)
                {
                    _studentsList.Add(new GetStudentsDto
                    {
                        Id = student.Id,
                        StudentName = student.StudentName
                    });
                }
            }

            var _teacherWithStudent = new GetTeacherWithStudentsDto
            {
                Id = teacher.Id,
                TeacherName = teacher.TeacherName,
                Students = _studentsList
            };
            teacherWithStudents.Add(_teacherWithStudent);
        }

        if (teacherWithStudents.Count > 0)
        {
            return Ok(teacherWithStudents);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost(nameof(GetAllTeacherWithStudentsUsingProjection))]
    public async Task<ActionResult<IEnumerable<GetTeacherWithStudentsDto>>> GetAllTeacherWithStudentsUsingProjection()
    {
        var teachersFromDb = await _dbContext.Teacher.AsNoTracking()
                                                     .Include(x => x.Students)
                                                     .ProjectTo<GetTeacherWithStudentsDto>(_mapper.ConfigurationProvider)
                                                     .ToListAsync();

        if (teachersFromDb.Count > 0)
        {
            return Ok(teachersFromDb);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost(nameof(CreateAuthorAndBookUsingTransaction))]
    public async Task<ActionResult<bool>> CreateAuthorAndBookUsingTransaction()
    {
        var authorWithBook = new Author
        {
            AuthorName = "Jane Austen",
            Books = new List<Book>
            {
                new Book
                {
                    BookName = "Pride and Prejudice",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            ReviewTitle = "5 star Rated",
                            ReviewDescription = "This book has 5 star rating overall"
                        }
                    }
                }
            }
        };

        var bookWithAuthor = new Book
        {
            BookName = "The Kite Runner",
            Reviews = new List<Review>
            {
                new Review
                {
                   ReviewTitle = "5 star Rated",
                   ReviewDescription = "This book has 5 star rating overall"
                }
            },
            Authors = new List<Author>
            {
                new Author
                {
                    AuthorName = "Khaled Hosseini"
                }
            }
        };

        using var transaction = await _dbContextAdvance.Database.BeginTransactionAsync();
        await _dbContextAdvance.Author.AddAsync(authorWithBook);
        await _dbContextAdvance.SaveChangesAsync();

        await _dbContextAdvance.Book.AddAsync(bookWithAuthor);
        await _dbContextAdvance.SaveChangesAsync();

        await transaction.CommitAsync();

        //using (var transaction = new TransactionScope(
        //   TransactionScopeOption.Required,
        //   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
        //{
        //    await _dbContextAdvance.Author.AddAsync(authorWithBook);
        //    await _dbContextAdvance.SaveChangesAsync();

        //    await _dbContextAdvance.Book.AddAsync(bookWithAuthor);
        //    await _dbContextAdvance.SaveChangesAsync();

        //    transaction.Complete();
        //}

        return true;
    }

    [HttpPost(nameof(BulkUpdateBooks))]
    public async Task<ActionResult<bool>> BulkUpdateBooks()
    {
        var result = await _dbContextAdvance.Book
                               .Where(x => !x.IsDeleted)
                               .ExecuteUpdateAsync(x => x.SetProperty(prop => prop.ModifiedBy, prop => "Admin"));
        if (result > 0)
        {
            return Ok(true);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost(nameof(BulkDeleteAuthorAndBooks))]
    public async Task<ActionResult<bool>> BulkDeleteAuthorAndBooks()
    {
        var authorResult = await _dbContextAdvance.Book
                               .Where(x => !x.IsDeleted)
                               .ExecuteDeleteAsync();

        var bookResult = await _dbContextAdvance.Author
                               .Where(x => !x.IsDeleted)
                               .ExecuteDeleteAsync();

        if (authorResult > 0 && bookResult > 0)
        {
            return Ok(true);
        }
        else
        {
            return BadRequest();
        }
    }
}