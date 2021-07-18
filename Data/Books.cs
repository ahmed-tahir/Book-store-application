using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Data
{
    public class Books
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int LanguageID { get; set; }
        public Languages Language { get; set; }
        public int TotalPages { get; set; }
        public string CoverImageUrl { get; set; }
        public string BookPdfUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<BooksGallery> BookGallery { get; set; }
    }
}
