using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Helpers
{
    public class CustomAttribute : ValidationAttribute
    {
        public CustomAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                string input = Convert.ToString(value);
                if (input.Contains(Text)) return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ?? "Value is empty");
            //return base.IsValid(value, validationContext);
        }
    }
}
