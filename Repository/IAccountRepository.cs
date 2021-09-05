using BookStoreApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<SignInResult> SignInAsync(SignInModel signInModel);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePasswordModel);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task GenerateForgotPasswordTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
    }
}