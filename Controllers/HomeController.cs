using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        [Route("~/")]
        public ViewResult Index()
        {
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
