using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    public enum TodoStatus
    {
        InComplete,
        Complete,
    }

    [Table("todoes")]
    public class Todo : AuditableEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("title")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("todo_status")]
        public TodoStatus Status { get; set; } = TodoStatus.InComplete;

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
