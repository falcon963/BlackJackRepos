using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Repositories.Interfaces
{
    public interface ILoginRepository
        : IBaseRepository<User>
    {
        User GetAppRegistrateUserAccount(string login, string password);
        string GetUserLogin(string login);
        int? GetSocialAccountUserId(User user);
    }
}
