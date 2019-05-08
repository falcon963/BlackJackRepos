using BooksLibrary.DAL.CustomValidationAtributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksLibrary.DAL.Entities
{
    public class Book
        : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [ADYear]
        [DataType(DataType.Date)]
        public int Year { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Range(0, 99999.99)]
        public string Price { get; set; }
    }
}
