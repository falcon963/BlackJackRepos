using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.DBConnection;

namespace TestProject.Core.Services
{
    public class LoginService: ILoginRepository
    {
        private readonly SqliteAppConnection _dbConnection;

        public LoginService(IDatabaseConnectionService connectionService)
        {
            _dbConnection = new SqliteAppConnection(connectionService);
        }

        public String HashPassword(String password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void SetLoginAndPassword(String login, String password)
        {
            CrossSecureStorage.Current.SetValue(SecureConstant.Login, login);
            CrossSecureStorage.Current.SetValue(SecureConstant.Password, password);

        }

        public User CheckAccountAccess(String login, String password)
        {
            User user = _dbConnection.Database.Table<User>().Where(v => v.Login == login && v.Password == password).FirstOrDefault();

            return user;
        }

        public Boolean CheckValidLogin(String login)
        {

            User result = _dbConnection.Database.Table<User>().Where(v => v.Login == login).FirstOrDefault();

            return result == null ? true : false;

        }

        public Boolean CreateUser(User user)
        {
            _dbConnection.Database.Insert(user);
            User getingUser = _dbConnection.Database.Table<User>().Where(v => v.Login == user.Login && v.Password == user.Password).First();
            if (getingUser != null)
            {
                return true;
            }
            return false;
        }
    }
}
