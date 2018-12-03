using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces
{
    public interface ILoginRepository
    {
        void SetLoginAndPassword(String login, String password);
        User CheckAccountAccess(String login, String password);
        Boolean CheckValidLogin(String login);
        Boolean CreateUser(User user);
    }
}
