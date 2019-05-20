using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    public interface IUserHelper
    {
        void DeleteUserStatus();
        void DeleteUserAccessToken();
        string UserLogin { get; set; }
        string UserPassword { get; set; }
        int UserId { get; set; }
        string UserAccessToken { get; set; }
        bool IsUserLogin { get; set; }
    }
}
