namespace WebAPI.Models
{
    public class LoginResponseDto
    {
        public string Message { get; set; }
        public UserDto User { get; set; }
        public string Token { get; set; }
        public string SessionId { get; set; } // Guid yerine string!
    }

}
