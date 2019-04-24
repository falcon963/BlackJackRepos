using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Models
{
    [Table("TasksFiles")]
    public class TaskFileModel
        : BaseModel
    {
        [ForeignKey(typeof(UserTask))]
        public int TaskId { get; set; }

        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileExtension { get; set; }

        [ManyToOne]
        public UserTask Task { get; set; }
    }
}
