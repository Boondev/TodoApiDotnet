using TodoApi.Models;

namespace TodoApi.Dtos.Todo;

public class GetTodoDto
{
    public TodoStatus Status { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}
