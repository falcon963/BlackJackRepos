using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Interfaces
{
    public interface IUserTask
    {
        Int32 Id { get; set; }
        String Title { get; set; }
        String Note { get; set; }
        Boolean Status { get; set; }
    }
}
