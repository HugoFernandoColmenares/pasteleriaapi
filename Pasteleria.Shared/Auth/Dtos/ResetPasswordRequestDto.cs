namespace Pasteleria.Shared.Auth.Dtos
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
