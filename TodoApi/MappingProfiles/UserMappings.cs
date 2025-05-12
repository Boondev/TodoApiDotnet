using AutoMapper;
using TodoApi.Dtos.User;
using TodoApi.Models;

namespace TodoApi.MappingProfiles
{
    public class UserMappings:Profile
    {
        public UserMappings()
        {
            CreateMap<User,RegisterDto>().ReverseMap();
        }
    }
}
