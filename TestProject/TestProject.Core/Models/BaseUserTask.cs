using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfaces;

namespace TestProject.Core.Models
{
    public class BaseUserTask : IUserTask
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Id { get; set; }
        [MaxLength(50)]
        public String Title { get; set; }
        public String Note { get; set; }
        public Boolean Status { get; set; }
    }
}
