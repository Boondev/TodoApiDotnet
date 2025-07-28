using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController(TodoDbContext context, IMapper mapper) : Controller
    {
        private TodoDbContext _context { get; set; } = context;
        private readonly IMapper _mapper = mapper;

        public IActionResult GetTodoList()
        {
            return View();
        }
    }
}
