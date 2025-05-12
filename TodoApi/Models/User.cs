using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [EmailAddress]
        [Column("email")]
        [MaxLength(255)]
        public string Email { get; set; }

        [PasswordPropertyText]
        [Column("password")]
        [MaxLength(500)]
        public string Password { get; set; }    

        [Column("name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
