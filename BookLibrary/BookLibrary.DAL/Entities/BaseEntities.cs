using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DAL.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreateDate = DateTime.Now;
        }

        public long Id { get; set; }
        public DateTime CreateDate { get; private set; }
    }
}
