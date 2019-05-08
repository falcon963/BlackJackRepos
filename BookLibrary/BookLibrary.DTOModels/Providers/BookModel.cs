using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DTOModels.Providers
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public ICollection<GenreModel> Genres { get; set; }
        public ICollection<AuthorModel> Authors { get; set; }
    }
}
