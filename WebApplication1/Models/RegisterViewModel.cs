using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Þifre zorunludur")]
        [MinLength(6, ErrorMessage = "Þifre en az 6 karakter olmalýdýr")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Þifre tekrar zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Þifreler eþleþmiyor")]
        public string ConfirmPassword { get; set; }
    }
}
