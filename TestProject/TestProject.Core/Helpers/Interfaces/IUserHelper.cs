using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    public interface IUserHelper
    {
        int GetUserId();
        bool GetUserStatus();
        void SetUserId(int userId);
        void SetUserStatus(bool status);
        void DeleteUserStatus();
        void DeleteUserAccessToken();
        string GetUserAccessToken();
        void SetUserLogin(string login);
        void SetUserPassword(string password);
        string GetUserLogin();
        string GetUserPassword();
    }
}
