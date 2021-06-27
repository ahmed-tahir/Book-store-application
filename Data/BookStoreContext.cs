using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext()
        {

        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }

        // This cpiece of code can also be written in the startup class under ConfigureServices() method.
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=TAHIRAHMEDT_I5;Database=BookStore;Integrated Security=True;");
        //    base.OnConfiguring(optionsBuilder); 
        //}

        public DbSet<Books> Books { get; set; }

    }
}
