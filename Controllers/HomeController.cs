using BookStoreApplication.Models;
using BookStoreApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public HomeController(IConfiguration configuration, IUserService userService, IEmailService emailService)
        {
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
        }

        [Route("~/")]
        public ViewResult Index()
        {
            // Reading configuration values from appsettings.json
            var environment = _configuration["Environment"];
            var applicationName = _configuration["ApplicationName"];
            var authorName = _configuration["Author:Name"];

            /* GetValue() */
            //var displayAlert = _configuration.GetValue<bool>("NewBookAlert:DisplayNewBookAlert");
            //var bookName = _configuration.GetValue<string>("NewBookAlert:BookName");

            /* GetSection() */
            //var newBook = _configuration.GetSection("NewBookAlert");
            //var displayAlert = newBook.GetValue<bool>("DisplayNewBookAlert");
            //var bookName = newBook.GetValue<string>("BookName");

            /* Binding configurations to objects using Bind() */
            var newBookAlert = new NewBookAlertConfig();
            _configuration.Bind("NewBookAlert", newBookAlert);

            var displayAlert = newBookAlert.DisplayNewBookAlert;

            return View();
        }

        [Route("About-Us")]
        public ViewResult AboutUs()
        {
            var userID = _userService.GetUserId();
            bool isLoggedIn = _userService.IsAuthenticated();
            return View();
        }

        [Route("Contact-Us")]
        public async Task<ViewResult> ContactUs()
        {
            UserEmailOptions options = new UserEmailOptions() 
            { 
                ToEmails = new List<string>() { "tahirahmed.tapadhar@learningmate.com" },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", "Tahir")
                }
            };
            await _emailService.SendTestEmail(options);
            return View();
        }
    }
}
