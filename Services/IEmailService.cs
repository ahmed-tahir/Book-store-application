using BookStoreApplication.Models;
using System.Threading.Tasks;

namespace BookStoreApplication.Services
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions emailOptions);
        Task SendEmailConfirmation(UserEmailOptions emailOptions);
    }
}