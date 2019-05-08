using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DTOModels.Providers
{
    public class AuthorModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }

        public ICollection<BookModel> Books { get; set; }
    }
}
