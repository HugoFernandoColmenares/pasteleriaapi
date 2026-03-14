using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Shared.Auth.Dtos
{
    public class UserForggotPasswordRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
