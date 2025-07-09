using AutoMapper;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Dtos.User;
using TodoApi.Models;
using TodoApi.Utils;

namespace TodoApi.Services;

public interface IAuthService
{
    UserDto Register(RegisterDto dto);
}

public class AuthService(TodoDbContext _context, IMapper _mapper) : IAuthService
{
    public UserDto Register(RegisterDto dto)
    {
        User user = _mapper.Map<User>(dto);
        var hasher = new PasswordHasher();
        user.Password = hasher.HashPassword(user, user.Password);
        _context.Users.Add(user);
        if (_context.SaveChanges() < 0)
        {
            throw new Exception("cant register user");
        }

        User? newUser = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
        return _mapper.Map<UserDto>(newUser);
    }
}
