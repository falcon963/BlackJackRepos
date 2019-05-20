using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DAL.Entities
{
    public class BookGenre
    {
        public long BookId { get; set; }
        public Book Book { get; set; }

        public long GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
