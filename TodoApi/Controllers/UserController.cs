using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos.User;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class UserController(TodoDbContext context, IMapper mapper) : Controller
    {
        private TodoDbContext _context { get; set; } = context;
        private readonly IMapper _mapper = mapper;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] RegisterDto Dto)
        {

            if (Dto == null)
            {
                return BadRequest();
            }
            var ExistingUser = _context.Users.FirstOrDefault(x => x.Email == Dto.Email);

            if (ExistingUser != null)
            {
                return UnprocessableEntity("Email has been used");
            }

            User user = _mapper.Map<User>(Dto);

            _context.Users.Add(user);
            if (_context.SaveChanges()<0)
            {
                throw new Exception("cant register user");
            }

            User newUser = _context.Users.FirstOrDefault(x => x.Email == Dto.Email);

            return Ok(newUser);
        }
    }
}
