using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos.User
{
    public class LoginDto
    {
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
