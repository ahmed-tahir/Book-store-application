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
        public async Task<IActionResult> Signup(SignUpUserModel model)
        {
            if(ModelState.IsValid)
            {
                // inserting user data into identity table
                var result = await _accountRepository.CreateUserAsync(model);
                if(!result.Succeeded)
                {
                    foreach(var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(model);
                }
                ModelState.Clear();
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
                ModelState.AddModelError("", "Invalid credentials");
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
    }
}
