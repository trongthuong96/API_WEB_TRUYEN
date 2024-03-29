﻿using API.Models;
using API.Models.Models;
using API.Models.Models.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mapper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Story, StoryDto>().ReverseMap();
            CreateMap<Story, StoryCreateDto>().ReverseMap();
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<CategoryStory, CategoryStoryDto>().ReverseMap();
            CreateMap<Chapter, ChapterCreateDto>().ReverseMap();
            CreateMap<Chapter, ChapterDto>().ReverseMap();
            CreateMap<Chapter, ChapterUpdateDto>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserCreateDto>().ReverseMap();
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();
            CreateMap<Tick, TickDto>().ReverseMap();
            CreateMap<Comment, CommentCreateDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentDeleteDto>().ReverseMap();
        }
    }
}
