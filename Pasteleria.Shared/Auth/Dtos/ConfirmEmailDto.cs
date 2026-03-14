namespace Pasteleria.Shared.Auth.Dtos
{
    public class ConfirmEmailDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
