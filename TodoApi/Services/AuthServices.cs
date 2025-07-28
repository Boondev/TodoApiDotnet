using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Dtos.User;
using TodoApi.Models;
using TodoApi.Utils;
using TodoApi.Utils.Exceptions;

namespace TodoApi.Services;

public interface IAuthService
{
    UserDto Register(RegisterDto dto);
    string Login(LoginDto dto);
}

public class AuthService(TodoDbContext _context, IMapper _mapper, IConfiguration _configuration)
    : IAuthService
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

    public string Login(LoginDto dto)
    {
        User? user =
            _context.Users.First(x => x.Email == dto.Email)
            ?? throw new UnprocessableEntityException("Account or password incorrect");

        var hasher = new PasswordHasher();
        bool verify =
            hasher.VerifyHashedPassword(user, dto.Password) == PasswordVerificationResult.Success;

        if (!verify)
            throw new UnprocessableEntityException("Account or password incorrect");

        List<Claim> authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var token = generateJWTToken(authClaims);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken generateJWTToken(List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(7),
            claims: claims,
            signingCredentials: new SigningCredentials(
                authSigningKey,
                SecurityAlgorithms.HmacSha256
            )
        );

        return token;
    }
}
