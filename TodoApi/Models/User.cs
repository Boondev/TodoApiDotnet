using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    [Table("users")]
    public class User : AuditableEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [EmailAddress]
        [Column("email")]
        [MaxLength(255)]
        public string Email { get; set; }

        [JsonIgnore]
        [PasswordPropertyText]
        [Column("password")]
        [MaxLength(500)]
        public string Password { get; set; }

        [Column("name")]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
