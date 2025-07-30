using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Dtos.Todo;
using TodoApi.Dtos.User;
using TodoApi.Models;

namespace TodoApi.MappingProfiles
{
    public class TodoMappings : Profile
    {
        public TodoMappings()
        {
            CreateMap<Todo, CreateTodoDto>().ReverseMap();
            CreateMap<Todo, UpdateTodoDto>().ReverseMap();
            CreateMap<Todo, Todo>();
        }
    }
}
