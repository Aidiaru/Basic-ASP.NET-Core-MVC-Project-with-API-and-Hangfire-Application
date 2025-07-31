using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Ge�erli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "�ifre zorunludur")]
        [MinLength(6, ErrorMessage = "�ifre en az 6 karakter olmal�d�r")]
        public string Password { get; set; }

        [Required(ErrorMessage = "�ifre tekrar zorunludur")]
        [Compare("Password", ErrorMessage = "�ifreler e�le�miyor")]
        public string ConfirmPassword { get; set; }
    }
}