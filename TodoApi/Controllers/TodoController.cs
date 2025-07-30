using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Dtos.Todo;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController(ITodoService todoService) : Controller
    {
        private readonly ITodoService _todoService = todoService;

        [Authorize]
        [HttpPost("get")]
        public IActionResult GetTodoList([FromBody] GetTodoDto getTodoDto)
        {
            Console.WriteLine("halo");
            var user = HttpContext.Items["User"] as User ?? throw new UnauthorizedAccessException();
            var list = _todoService.GetTodoList(user, getTodoDto);
            Console.WriteLine(list);
            return Ok(list);
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateTodo([FromBody] CreateTodoDto dto)
        {
            var user = HttpContext.Items["User"] as User ?? throw new UnauthorizedAccessException();
            _todoService.CreateTodo(user, dto);
            return Ok();
        }

        [Authorize]
        [HttpPatch("edit")]
        public IActionResult EditTodo([FromBody] UpdateTodoDto dto)
        {
            var user = HttpContext.Items["User"] as User ?? throw new UnauthorizedAccessException();
            _todoService.EditTodo(user.Id, dto);
            return Ok();
        }

        [Authorize]
        [HttpPatch("{id}/mark")]
        public IActionResult MarkTodo(int id)
        {
            var user = HttpContext.Items["User"] as User ?? throw new UnauthorizedAccessException();
            _todoService.MarkTodo(user.Id, id);
            return Ok();
        }

        [Authorize]
        [HttpPatch("{id}/unmark")]
        public IActionResult UnMarkTodo(int id)
        {
            var user = HttpContext.Items["User"] as User ?? throw new UnauthorizedAccessException();
            _todoService.UnMarkTodo(user.Id, id);
            return Ok();
        }

        [Authorize]
        [HttpPatch("{id}/delete")]
        public IActionResult DeleteTodo(int id)
        {
            var user = HttpContext.Items["User"] as User ?? throw new UnauthorizedAccessException();
            _todoService.DeleteTodo(user.Id, id);
            return Ok();
        }
    }
}
