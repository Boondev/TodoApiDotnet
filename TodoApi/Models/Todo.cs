using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public enum TodoStatus
    {
        Complete,
        InComplete
    }

    [Table("todoes")]
    public class Todo
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("title")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("category")]
        [MaxLength(200)]
        public string Category { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
        [Column("todo_status")]
        public TodoStatus Status { get; set; }
        public virtual User User { get; set; }
    }
}
