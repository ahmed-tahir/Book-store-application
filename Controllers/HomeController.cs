using BookStoreApplication.Models;
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

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
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
            return View();
        }

        [Route("Contact-Us")]
        public ViewResult ContactUs()
        {
            return View();
        }
    }
}
