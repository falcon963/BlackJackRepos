using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TestProject.Core.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Id { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String ImagePath { get; set; }
    }
}
