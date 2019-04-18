using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Models
{
    [Table("TaskFile")]
    public class TaskFileModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileExtension { get; set; }
    }
}
