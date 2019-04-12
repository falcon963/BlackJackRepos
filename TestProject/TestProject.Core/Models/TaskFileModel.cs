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
        public Int32 Id { get; set; }
        public Int32 TaskId { get; set; }
        public String FileName { get; set; }
        public Byte[] FileContent { get; set; }
        public String FileExtension { get; set; }
    }
}
