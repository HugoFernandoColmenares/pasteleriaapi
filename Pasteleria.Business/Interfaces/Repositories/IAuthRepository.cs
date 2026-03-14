using Microsoft.AspNetCore.Identity;
using Pasteleria.Shared.Models;

namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByIdAsync(string userId);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);
    }
}
