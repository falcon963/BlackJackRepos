using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksLibrary.DAL.Entities
{
    public class Genre
        : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
