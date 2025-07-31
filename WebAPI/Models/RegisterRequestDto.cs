using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Þifre zorunludur")]
        [MinLength(6, ErrorMessage = "Þifre en az 6 karakter olmalýdýr")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Þifre tekrar zorunludur")]
        [Compare("Password", ErrorMessage = "Þifreler eþleþmiyor")]
        public string ConfirmPassword { get; set; }
    }
}