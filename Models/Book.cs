using BookStoreApplication.Enums;
using BookStoreApplication.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Models
{
    public class Book
    {
        public int ID { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Please enter the title of your book")]
        //[Custom("Custom error for custom attribute")]
        public string Title { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required(ErrorMessage = "Please enter the author name")]
        public string Author { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        [Required(ErrorMessage = "Please select the language of your book")]
        public int LanguageID { get; set; }

        public string Language { get; set; }

        [Display(Name = "Total Pages")]
        [Required(ErrorMessage = "Please enter the total pages")]
        public int? TotalPages { get; set; }
    }
}
