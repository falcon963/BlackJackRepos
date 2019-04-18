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
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }
    }
}
