using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    interface IPasswordHelper
    {
        string HashPassword(string password);
    }
}
