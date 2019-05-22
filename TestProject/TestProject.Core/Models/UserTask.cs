﻿using SQLite;
using SQLiteNetExtensions.Attributes;
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
            ;
        }

        public UserTask(UserTask userTask)
        {
            Id = userTask.Id;
            UserId = userTask.Id;
            ImagePath = userTask.ImagePath;
            Note = userTask.Note;
            Title = userTask.Title;
            Status = userTask.Status;
        }

        [ForeignKey(typeof(User))]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TaskFileModel> Files { get; set; }

        [ManyToOne]
        public User User { get; set; }

        #region overrides

        public override bool Equals(object obj)
        {
            var task = obj as UserTask;
            return task != null &&
                   Title == task.Title &&
                   Note == task.Note &&
                   Status == task.Status &&
                   ImagePath == task.ImagePath;
        }

        public override int GetHashCode()
        {
            var hashCode = -1936418545;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Note);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(Status);//Status.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ImagePath);
            return hashCode;
        }
        #endregion
    }
}
