using BooksLibrary.DAL.CustomValidationAtributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksLibrary.DAL.Entities
{
    public class Author
        : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [ADYear]
        [DataType(DataType.Date)]
        public int Year { get; set; }
        [Required]
        [StringLength(100)]
        public string Country { get; set; }
    }
}
