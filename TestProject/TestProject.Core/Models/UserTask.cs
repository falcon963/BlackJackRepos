using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfaces;

namespace TestProject.Core.Models
{
    [Table("UserTask")]
    public class UserTask : BaseUserTask
    {
        [PrimaryKey, AutoIncrement]
        public new Int32 Id { get; set; }
    }
}
