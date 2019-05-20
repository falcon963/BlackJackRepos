using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksLibrary.DAL.CustomValidationAtributes
{
    public class ADYearAttribute
        : ValidationAttribute
    {
        private int _year;

        public ADYearAttribute()
        {
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _year = ((DateTime)value).Year;

            if(_year < 0 || _year > DateTime.Now.Year)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Incorrect date, can`t set {_year} year.";
        }
    }
}
