using BookStoreApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    public class AccountController : Controller
    {
        [Route("Signup")]
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("Signup")]
        [HttpPost]
        public IActionResult Signup(SignUpUserModel model)
        {
            if(ModelState.IsValid)
            {
                // inserting user data into identity table
            }
            return View();
        }
    }
}
