using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Dtos;

public class UserDto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [EmailAddress]
    [Column("email")]
    [MaxLength(255)]
    public string Email { get; set; }

    [Column("name")]
    [MaxLength(255)]
    public string Name { get; set; }
}
