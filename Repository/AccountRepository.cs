using BookStoreApplication.Models;
using BookStoreApplication.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUserService _userService;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
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
            return result;
        }

        /// <summary>
        /// This method is responsible for logging in the user
        /// </summary>
        /// <param name="signInModel"></param>
        /// <returns></returns>
        public async Task<SignInResult> SignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, false);
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
    }
}
