using BookStoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Repository
{
    public class BookRepository
    {
        public List<Book> GetAllBooks()
        {
            return DataSource();
        }

        public Book GetBookById(int id)
        {
            return DataSource().Where(x => x.ID == id).FirstOrDefault();
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
