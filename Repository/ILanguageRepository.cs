using BookStoreApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public interface ILanguageRepository
    {
        Task<List<Language>> GetLanguages();
    }
}