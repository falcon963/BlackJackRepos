using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces
{
    public interface ILoginService
    {
        void SetLoginAndPassword(String login, String password);
        User CheckAccountAccess(String login, String password);
        Boolean CheckValidLogin(String login);
        Int32 CreateUser(User user);
        Int32 GetSocialAccount(User user);
        User TakeProfile(Int32 userId);
        void ChangePassword(Int32 userId, String password);
        void ChangeImage(Int32 userId, String imagePath);
    }
}
