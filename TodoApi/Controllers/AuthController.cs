﻿using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Dtos.User;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Utils;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(TodoDbContext _context, IMapper _mapper, IAuthService authService)
        : Controller
    {
        [HttpPost("register")]
        public ActionResult<UserDto> Register([FromBody] RegisterDto Dto)
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

            UserDto userDto = authService.Register(Dto);

            return Ok(userDto);
        }

        [HttpPost("login")]
        public ActionResult<UserDto> Login([FromBody] LoginDto Dto)
        {
            if (Dto == null)
            {
                return UnprocessableEntity("Please fill in the information requried");
            }

            string token = authService.Login(Dto);

            return Ok(token);
        }
    }
}
