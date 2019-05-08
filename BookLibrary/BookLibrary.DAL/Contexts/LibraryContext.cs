using BooksLibrary.DAL.Entities;
using BooksLibrary.DTOModels.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DAL.Contexts
{
    public class LibraryContext
        : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }

        public LibraryContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BooksLibrary;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookGenre>()
                .HasOne<Book>(bg => bg.Book)
                .WithMany()
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<BookGenre>()
                .HasOne<Genre>(bg => bg.Genre)
                .WithMany()
                .HasForeignKey(bg => bg.GenreId);

            modelBuilder.Entity<AuthorBook>()
                .HasOne<Author>(bg => bg.Author)
                .WithMany()
                .HasForeignKey(bg => bg.AuthorId);

            modelBuilder.Entity<AuthorBook>()
                .HasOne<Book>(bg => bg.Book)
                .WithMany()
                .HasForeignKey(bg => bg.BookId);
        }
    }
}
