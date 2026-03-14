using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pasteleria.Shared.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Company { get; set; } = "Oniriums";
    }
}
