using BookStoreApplication.Models;
using BookStoreApplication.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("Signup")]
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("Signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUserModel userModel)
        {
            if(ModelState.IsValid)
            {
                // inserting user data into identity table
                var result = await _accountRepository.CreateUserAsync(userModel);
                if(!result.Succeeded)
                {
                    foreach(var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                // redirect user to email confirmation view
                return RedirectToAction("ConfirmEmail", new { email = userModel });
            }
            return View();
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model, string returnURL)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.SignInAsync(model);
                if(result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnURL))
                        return LocalRedirect(returnURL);
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsNotAllowed) ModelState.AddModelError("", "You are not allowed to login");
                else ModelState.AddModelError("", "Invalid credentials");
            }
            return View();
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Change-password")]
        public IActionResult ChangePassword()
        {
            ViewBag.PasswordChangeStatus = false;
            return View();
        }

        [HttpPost]
        [Route("Change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);
                if(result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.PasswordChangeStatus = true;
                    return View();
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("Confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmationModel model = new EmailConfirmationModel()
            {
                Email = email
            };

            if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if(result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if(user != null)
            {
                if(user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }
                // If user has not confirmed their email, then resend the verification email
                var result = _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            return View(model);
        }
    }
}
