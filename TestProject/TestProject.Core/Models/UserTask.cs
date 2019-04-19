using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfacies;

namespace TestProject.Core.Models
{
    [Table("UserTask")]
    public class UserTask
        : BaseModel
    {
        public int UserId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
