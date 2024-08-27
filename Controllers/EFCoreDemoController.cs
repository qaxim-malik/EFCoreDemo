using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreDemo.Domain.Context;
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
    private readonly IMapper _mapper;

    public EFCoreDemoController(EFCoreDemoContext dbContext,
                                IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
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
            return teacherEntity.Id;
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
            TeacherId = student.TeacherId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Admin"
        };
        await _dbContext.Student.AddAsync(studentEntity);
        var result = await _dbContext.SaveChangesAsync();
        if (result > 0)
        {
            return studentEntity.Id;
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost(nameof(GetAllTeacherWithStudents))]
    public async Task<ActionResult<IEnumerable<GetTeacherWithStudentsDto>>> GetAllTeacherWithStudents()
    {
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
            return teacherWithStudents;
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
            return teachersFromDb;
        }
        else
        {
            return NotFound();
        }
    }
}