using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApplication.Models;
using BookStoreApplication.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreApplication.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository = null;
        private readonly LanguageRepository _languageRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        public BookController(BookRepository bookRepository, LanguageRepository languageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return View(books);
        }

        public async Task<IActionResult> GetBook(int id)
        {
            var data = await _bookRepository.GetBookById(id);
            return View(data);
        }

        public List<Book> SearchBook(string bookname, string authorName)
        {
            return _bookRepository.SearchBook(bookname, authorName);
        }

        public async Task<ViewResult> AddNewBook(bool isSuccess = false, int bookID = 0)
        {
            // Populating dropdown values from database
            ViewBag.Language = new SelectList(await _languageRepository.GetLanguages(), "ID", "Name");

            //ViewBag.Language = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "English", Value = "1"},
            //    new SelectListItem(){ Text = "Hindi"  , Value = "2"},
            //    new SelectListItem(){ Text = "Spanish", Value = "3"},
            //    new SelectListItem(){ Text = "Dutch"  , Value = "4"},
            //    new SelectListItem(){ Text = "French" , Value = "5"},
            //    new SelectListItem(){ Text = "Mandarin", Value = "6"}
            //};

            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookID = bookID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBook(Book bookModel)
        {
            if(ModelState.IsValid)
            {
                // saving the cover image to server
                if(bookModel.CoverPhoto != null)
                {
                    string folderURL = "books/cover/";
                    folderURL += Guid.NewGuid().ToString() + "_" + bookModel.CoverPhoto.FileName;
                    // server path where image will be saved
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderURL);
                    bookModel.CoverImageUrl = folderURL;

                    await bookModel.CoverPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                int id = await _bookRepository.AddNewBook(bookModel);
                if(id > 0) 
                    return RedirectToAction(nameof(AddNewBook), new { isSuccess = true, bookID = id});
                return View();
            }

            ViewBag.Language = new SelectList(await _languageRepository.GetLanguages(), "ID", "Name");
            //ViewBag.Language = new SelectList(GetLanguage(), "ID", "Text");
            // Adding custom model error message
            //ModelState.AddModelError("", "This is my custom error message");

            return View();
        }

    }
}
