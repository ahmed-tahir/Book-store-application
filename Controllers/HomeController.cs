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
