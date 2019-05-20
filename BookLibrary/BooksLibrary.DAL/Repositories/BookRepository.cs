using BooksLibrary.DAL.Contexts;
using BooksLibrary.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DAL.Repositories
{
    public class BookRepository
        : BaseRepository<Book>
    {

        public BookRepository(LibraryContext libraryContext) : base(libraryContext)
        {

        }
    }
}
