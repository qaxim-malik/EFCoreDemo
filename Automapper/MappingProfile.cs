﻿using AutoMapper;
using EFCoreDemo.Domain.Entities.AdvanceEntities;
using EFCoreDemo.Domain.Entities.SimpleEntities;
using EFCoreDemo.Dto;

namespace EFCoreDemo.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Teacher, GetTeacherWithStudentsDto>();
        CreateMap<Student, GetStudentsDto>();

        CreateMap<Author, GetAuthorDto>();
        CreateMap<Book, GetBookDto>();
    }
}