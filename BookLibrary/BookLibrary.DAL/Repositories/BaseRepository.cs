using BooksLibrary.DAL.Contexts;
using BooksLibrary.DAL.Entities;
using BooksLibrary.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DAL.Repositories
{
    public class BaseRepository<TEntity>
        : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly LibraryContext _libraryContext;

        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(LibraryContext repositoryContext)
        {
            _libraryContext = repositoryContext;
            _dbSet = _libraryContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
        }

        public void Update(TEntity item)
        {
            _dbSet.Update(item);
        }

        public void Delete(int id)
        {
            TEntity entity = Get(id);

            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
