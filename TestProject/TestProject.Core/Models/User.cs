using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TestProject.Core.Models
{
    [Table("Users")]
    public class User
        : BaseModel
    {        
        public string Login { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<UserTask> Tasks { get; set; }
    }
}
