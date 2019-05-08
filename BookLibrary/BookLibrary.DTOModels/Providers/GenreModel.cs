using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DTOModels.Providers
{
    public class GenreModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BookModel> Books { get; set; }
    }
}
