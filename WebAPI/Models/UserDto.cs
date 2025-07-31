using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}