using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos.User
{
    public class RegisterDto
    {
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        public string Password { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
    }
}
