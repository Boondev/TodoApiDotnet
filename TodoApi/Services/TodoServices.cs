using AutoMapper;
using TodoApi.Data;
using TodoApi.Dtos.Todo;
using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    public List<Todo> GetTodoList(User user, GetTodoDto dto);

    public void CreateTodo(User user, CreateTodoDto dto);

    public void EditTodo(int userId, UpdateTodoDto todo);

    public void MarkTodo(int userId, int id);
    public void UnMarkTodo(int userId, int id);

    public void DeleteTodo(int userId, int id);
}

public class TodoService(TodoDbContext context, IMapper mapper) : ITodoService
{
    private readonly TodoDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public List<Todo> GetTodoList(User user, GetTodoDto dto)
    {
        var todoes = _context.Todoes.Where(
            (x) =>
                x.User.Id == user.Id
                && dto.Status == x.Status
                && (dto.Start == null || x.CreatedAt >= dto.Start)
                && (dto.End == null || x.CreatedAt <= dto.End)
        );

        return todoes.ToList();
    }

    public void CreateTodo(User user, CreateTodoDto dto)
    {
        Todo todo = _mapper.Map<Todo>(dto);
        todo.User = user;
        _context.Todoes.Add(todo);
        if (_context.SaveChanges() < 0)
            throw new Exception("create todo error");
    }

    public void EditTodo(int userId, UpdateTodoDto dto)
    {
        Todo todo = _mapper.Map<Todo>(dto);
        UpdateTodo(userId, todo, "update todo error");
    }

    public void MarkTodo(int userId, int id)
    {
        Todo todo = _context.Todoes.First(x => x.Id == id) ?? throw new Exception("todo not found");
        todo.Status = TodoStatus.Complete;
        UpdateTodo(userId, todo, "mark todo error");
    }

    public void UnMarkTodo(int userId, int id)
    {
        Todo todo = _context.Todoes.First(x => x.Id == id) ?? throw new Exception("todo not found");
        todo.Status = TodoStatus.InComplete;
        UpdateTodo(userId, todo, "unmark todo error");
    }

    public void DeleteTodo(int userId, int id)
    {
        Todo todo =
            _context.Todoes.First(x => x.Id == id && x.User.Id == userId)
            ?? throw new Exception("todo not found");
        _context.Todoes.Remove(todo);

        if (_context.SaveChanges() < 0)
            throw new Exception("delete todo error");
    }

    private void UpdateTodo(int userId, Todo todo, string errorMessage = "update error")
    {
        var findTodo =
            _context.Todoes.First(x => x.Id == todo.Id && x.User.Id == userId)
            ?? throw new Exception("todo not found");
        findTodo.Title = todo.Title;
        findTodo.Description = todo.Description;
        findTodo.Status = todo.Status;
        if (_context.SaveChanges() < 0)
            throw new Exception(errorMessage);
    }
}
