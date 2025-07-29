using TodoApi.Models;

namespace TodoApi.Dtos.Todo;

public class CreateTodoDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TodoStatus Status { get; set; }
}
