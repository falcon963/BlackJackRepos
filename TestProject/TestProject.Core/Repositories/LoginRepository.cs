using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using TestProject.Core.Providers;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Providers.Interfacies;
using TestProject.Core.Helpers.Interfaces;
using MvvmCross;

namespace TestProject.Core.Repositories
{
    public class LoginRepository
        : BaseRepository<User>, ILoginRepository
    {

        public LoginRepository(IDatabaseConnectionProvider dbConnection) : base(dbConnection)
        {

        }

        public User GetAppRegistrateUserAccount(string login, string password)
        {
            var user = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.Login == login && v.Password == password);

            return user;
        }

        public int? GetSocialAccountUserId(User user)
        {
            User getUser = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.FacebookId == user.FacebookId || v.GoogleId == user.GoogleId);

            return getUser?.Id;
        }


        public string GetUserLogin(string login)
        {
            User user = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.Login == login);

            string userLogin = user?.Login;

            return userLogin;

        }
    }
}
