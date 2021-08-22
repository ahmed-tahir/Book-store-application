using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetUserId()
        {
            string user = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return user;
        }

        public bool IsAuthenticated()
        {
            bool status = _httpContext.HttpContext.User.Identity.IsAuthenticated;
            return status;
        }
    }
}
