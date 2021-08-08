using BookStoreApplication.Data;
using BookStoreApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context = null;

        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewBook(BookModel model)
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
                CoverImageUrl = model.CoverImageUrl,
                BookPdfUrl = model.BookPdfUrl
            };

            // adding gallery images
            newBook.BookGallery = new List<BooksGallery>();
            foreach (var file in model.Gallery)
            {
                newBook.BookGallery.Add(new BooksGallery() { Name = file.Name, URL = file.URL });
            }
            await _context.Books.AddAsync(newBook);
            var response = await _context.SaveChangesAsync();
            return newBook.ID;
        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            return await _context.Books.Select(book => new BookModel()
            {
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                ID = book.ID,
                LanguageID = book.LanguageID,
                Language = _context.Languages.Where(x => x.ID == book.ID).Select(x => x.Name).FirstOrDefault(),
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl
            }).ToListAsync();
        }

        public async Task<List<BookModel>> GetTopBooksAsync(int count)
        {
            return await _context.Books.Select(book => new BookModel()
            {
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                ID = book.ID,
                LanguageID = book.LanguageID,
                Language = _context.Languages.Where(x => x.ID == book.ID).Select(x => x.Name).FirstOrDefault(),
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl
            }).Take(count).ToListAsync();
        }

        public async Task<BookModel> GetBookById(int id)
        {
            var book = await _context.Books.Where(b => b.ID == id).Select(book => new BookModel()
            {
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                ID = book.ID,
                LanguageID = book.LanguageID,
                Language = book.Language.Name,
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl,
                BookPdfUrl = book.BookPdfUrl,
                Gallery = book.BookGallery.Select(g => new BookGallery() { ID = g.ID, Name = g.Name, URL = g.URL }).ToList()
            }).FirstOrDefaultAsync();
            return book;
        }

        public List<BookModel> SearchBook(string title, string author)
        {
            return null;
        }

    }

}
