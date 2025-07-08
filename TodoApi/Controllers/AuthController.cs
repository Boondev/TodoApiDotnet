using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Dtos.User;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(TodoDbContext _context, IMapper _mapper) : Controller
    {
        const int keySize = 64;
        const int iterations = 300000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto Dto)
        {
            if (Dto == null)
            {
                return UnprocessableEntity("Please fill in the information requried");
            }
            var ExistingUser = _context.Users.FirstOrDefault(x => x.Email == Dto.Email);

            if (ExistingUser != null)
            {
                return UnprocessableEntity("Email has been used");
            }

            User user = _mapper.Map<User>(Dto);
            user.Password = EncryptPassword(user.Password, out byte[] salt);
            user.Salt = salt;
            _context.Users.Add(user);
            if (_context.SaveChanges() < 0)
            {
                throw new Exception("cant register user");
            }

            User? newUser = _context.Users.FirstOrDefault(x => x.Email == Dto.Email);

            return Ok(_mapper.Map<UserDto>(newUser));
        }

        private string EncryptPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            string hashed = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password,
                    salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: iterations,
                    numBytesRequested: 256 / 8
                )
            );
            return hashed;
        }
    }
}
