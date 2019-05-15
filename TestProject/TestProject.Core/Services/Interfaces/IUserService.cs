using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Services.Interfaces
{
    public interface IUserService
    {
        int LoginInSocialAccount(User user);
        void ChangeImage(int userId, string imagePath);
        void ChangePassword(int userId, string password);
        bool IsValidLogin(string login);

        void Logout();
    }
}
