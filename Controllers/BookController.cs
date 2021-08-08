using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApplication.Models;
using BookStoreApplication.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreApplication.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository = null;
        private readonly ILanguageRepository _languageRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        public BookController(IBookRepository bookRepository, ILanguageRepository languageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("All-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return View(books);
        }

        [Route("book-details/{id:int}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var data = await _bookRepository.GetBookById(id);
            return View(data);
        }

        public List<BookModel> SearchBook(string bookname, string authorName)
        {
            return _bookRepository.SearchBook(bookname, authorName);
        }

        [HttpGet]
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
        public async Task<IActionResult> AddNewBook(BookModel bookModel)
        {
            if(ModelState.IsValid)
            {
                // saving the cover image to server and database
                if(bookModel.CoverPhoto != null)
                {
                    string folderURL = "books/cover/";
                    bookModel.CoverImageUrl = await UploadImageToServer(folderURL, bookModel.CoverPhoto);
                }
                // saving gallery images to server and database
                if (bookModel.GalleryImages != null)
                {
                    string folderURL = "books/gallery/";
                    bookModel.Gallery = new List<BookGallery>();
                    foreach(var file in bookModel.GalleryImages)
                    {
                        var gallery = new BookGallery() 
                        {
                            Name = file.FileName,
                            URL = await UploadImageToServer(folderURL, file)
                        };
                        bookModel.Gallery.Add(gallery);
                    }
                }
                // saving the book pdf to server and database
                if(bookModel.BookPdf != null)
                {
                    string folderURL = "books/pdf/";
                    bookModel.BookPdfUrl = await UploadImageToServer(folderURL, bookModel.BookPdf);
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

        private async Task<string> UploadImageToServer(string folderPath, IFormFile file)
        {            
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            // server path where image will be saved
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return String.Concat("/", folderPath);
        }
    }
}
