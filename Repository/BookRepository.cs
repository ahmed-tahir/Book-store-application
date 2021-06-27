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
                Language = model.Language,
                Title = model.Title,
                TotalPages = model.TotalPages.HasValue ? model.TotalPages.Value : 0,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            await _context.Books.AddAsync(newBook);
            var response = await _context.SaveChangesAsync();
            return newBook.ID;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> books = new List<Book>();
            var allBooks = await _context.Books.ToListAsync();
            if(allBooks?.Any() == true)
            {
                foreach(Books item in allBooks)
                {
                    books.Add(new Book() 
                    {
                        Author = item.Author,
                        Category = item.Category,
                        Description = item.Description,
                        ID = item.ID,
                        Language = item.Language,
                        Title = item.Title,
                        TotalPages = item.TotalPages
                    });
                }
            }
            return books;
        }

        public async Task<Book> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book != null)
            {
                var bookDetails = new Book()
                {
                    Author = book.Author,
                    Category = book.Category,
                    Description = book.Description,
                    ID = book.ID,
                    Language = book.Language,
                    Title = book.Title,
                    TotalPages = book.TotalPages
                };
                return bookDetails;
            }
            return null;
        }

        public List<Book> SearchBook(string title, string author)
        {
            return DataSource().Where(x => x.Title.Contains(title) || x.Author.Contains(author)).ToList();
        }

        private List<Book> DataSource()
        {
            return new List<Book>()
            {
                new Book(){ID = 1, Author = "Andrew", Title = "Pro C#", Description="This is the description for Pro C# Book", Category="Programming", Language="English", TotalPages=450},
                new Book(){ID = 2, Author = "Kalen", Title = "SQL Internals", Description="This is the description for SQL Internals Book", Category="Framework", Language="English", TotalPages=335},
                new Book(){ID = 3, Author = "Ben", Title = "Pro Git", Description="This is the description for Pro GIT Book", Category="Version Control", Language="English", TotalPages=560},
                new Book(){ID = 4, Author = "Jeffrey", Title = "CLR Via C#", Description="This is the description for CLR Via C# Book", Category="Object-Oriented", Language="English", TotalPages=990},
                new Book(){ID = 5, Author = "Dhananjay", Title = "Angular Guide", Description="This is the description for Angular Book", Category="Front End", Language="English", TotalPages=625},
                new Book(){ID = 6, Author = "Scott", Title = "Azure DevOps", Description="This is the description for Azure Devops Book", Category="Cloud Computing", Language="English", TotalPages=1025}
            };
        }
    }

}
