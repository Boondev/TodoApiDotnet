using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

public class Session : AuditableEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    public string Text { get; set; }
    public DateTime ExpiredAt { get; set; }
    public virtual User User { get; set; }
}
