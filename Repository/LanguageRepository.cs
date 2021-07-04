using BookStoreApplication.Data;
using BookStoreApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public class LanguageRepository
    {
        private readonly BookStoreContext _context = null;

        public LanguageRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Language>> GetLanguages()
        {
            var languages = await _context.Languages.Select(x => new Language()
            {
                ID = x.ID,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return languages;
        }
    }
}
