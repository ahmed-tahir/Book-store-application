using BookStoreApplication.Data;
using BookStoreApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public class BookRepository
    {
        private readonly BookStoreContext _context = null;

        public BookRepository(BookStoreContext context)
        {
            _context = context;   
        }

        public async Task<int> AddNewBook(Book model)
        {
            var newBook = new Books()
            {
                Author = model.Author,
                Description = model.Description,
                LanguageID = model.LanguageID,
                Title = model.Title,
                TotalPages = model.TotalPages.HasValue ? model.TotalPages.Value : 0,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CoverImageUrl = model.CoverImageUrl
            };
            await _context.Books.AddAsync(newBook);
            var response = await _context.SaveChangesAsync();
            return newBook.ID;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books.Select(book => new Book() 
            {
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                ID = book.ID,
                LanguageID = book.LanguageID,
                Language = book.Language.Name,
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl
            }).ToListAsync(); 
        }

        public async Task<Book> GetBookById(int id)
        {
            var book = await _context.Books.Where(b => b.ID == id).Select(book => new Book()
            {
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                ID = book.ID,
                LanguageID = book.LanguageID,
                Language = book.Language.Name,
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl
            }).FirstOrDefaultAsync();
            return book;
        }

        public List<Book> SearchBook(string title, string author)
        {
            return null;
        }

    }

}
