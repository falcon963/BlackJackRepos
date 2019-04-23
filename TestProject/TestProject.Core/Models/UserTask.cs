using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Models
{
    [Table("UsersTasks")]
    public class UserTask
        : BaseModel
    {
        public UserTask()
        {

        }

        public UserTask(UserTask clon)
        {
            Id = clon.Id;
            UserId = clon.Id;
            ImagePath = clon.ImagePath;
            Note = clon.Note;
            Title = clon.Title;
            Status = clon.Status;
        }

        public int UserId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }

        public override bool Equals(object obj)
        {
            var task = obj as UserTask;
            return task != null &&
                   UserId == task.UserId &&
                   Title == task.Title &&
                   Note == task.Note &&
                   Status == task.Status &&
                   ImagePath == task.ImagePath;
        }

        public override int GetHashCode()
        {
            var hashCode = 994647589;
            hashCode = hashCode * -1521134295 + UserId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Note);
            hashCode = hashCode * -1521134295 + Status.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ImagePath);
            return hashCode;
        }
    }
}
