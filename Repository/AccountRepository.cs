using BookStoreApplication.Models;
using BookStoreApplication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
                                , IUserService userService, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Creates a user in the database using Identity core
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.Email,
                PhoneNumber = userModel.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if(result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(user);
            }
            return result;
        }

        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // send the token to the user's registered email id for confirmation
            await SendEmailConfirmationAsync(user, token);
        }

        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // send the token to the user's registered email id for confirmation
            await SendForgotPasswordAsync(user, token);
        }

        public async Task <ApplicationUser> GetUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        /// <summary>
        /// This method is responsible for logging in the user
        /// </summary>
        /// <param name="signInModel"></param>
        /// <returns></returns>
        public async Task<SignInResult> SignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, true);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// This method is used to change the current user password
        /// </summary>
        /// <param name="changePasswordModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePasswordModel)
        {
            string userID = _userService.GetUserId();
            ApplicationUser user = await _userManager.FindByIdAsync(userID);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
            return result;
        }

        /// <summary>
        /// This method sends an email for confirmation of email after user registration
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task SendEmailConfirmationAsync(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetValue<string>("Application:AppDomain");
            string confirmationLink = _configuration.GetValue<string>("Application:EmailConfirmation");

            UserEmailOptions options = new UserEmailOptions()
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", String.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendEmailConfirmation(options);
        }

        private async Task SendForgotPasswordAsync(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetValue<string>("Application:AppDomain");
            string confirmationLink = _configuration.GetValue<string>("Application:ForgotPassword");

            UserEmailOptions options = new UserEmailOptions()
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", String.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendEmailForForgotPassword(options);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.UserID);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result;
        }

        public async Task<IdentityResult> CreateRoleAsync(IdentityRole model)
        {
            var result = await _roleManager.CreateAsync(model);
            return result;
        }
    }
}
