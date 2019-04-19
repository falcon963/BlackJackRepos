using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Constants;
using TestProject.Core.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.DBConnection;
using TestProject.Core.Repositories.Interfacies;

namespace TestProject.Core.Repositories
{
    public class LoginRepository
        : ILoginRepository
    {
        private readonly SqliteAppConnection _dbConnection;

        public LoginRepository(IDatabaseConnectionService connectionService)
        {
            _dbConnection = new SqliteAppConnection(connectionService);
        }

        public void SetLoginAndPassword(string login, string password)
        {
            CrossSecureStorage.Current.SetValue(SecureConstant.Login, login);
            CrossSecureStorage.Current.SetValue(SecureConstant.Password, password);
        }

        public User GetAppRegistrateUserAccount(string login, string password)
        {
            return _dbConnection.Database.Table<User>().Where(v => v.Login == login && v.Password == password).FirstOrDefault();
        }

        public bool CheckValidLogin(string login)
        {

            User result = _dbConnection.Database.Table<User>().Where(v => v.Login == login).FirstOrDefault();

            return result == null ? true : false;

        }

        public void ChangePassword(int userId, string password)
        {
            User user = _dbConnection.Database.Table<User>().Where(u => u.Id == userId).FirstOrDefault();
            user.Password = password;
            _dbConnection.Database.Update(user);
        }
        public void ChangeImage(int userId, string imagePath)
        {
            User user = _dbConnection.Database.Table<User>().Where(u => u.Id == userId).FirstOrDefault();
            user.ImagePath = imagePath;
            _dbConnection.Database.Update(user);
        }

        public int GetSocialAccountUserId(User user)
        {
            User createdUser = _dbConnection.Database.Table<User>().Where(v => v.FacebookId == user.FacebookId || v.GoogleId == user.GoogleId).FirstOrDefault();
            if(createdUser != null)
            {
                return createdUser.Id;
            }
            if(createdUser == null)
            {
                _dbConnection.Database.Insert(user);
                User newUser = _dbConnection.Database.Table<User>().Where(v => v.FacebookId == user.FacebookId || v.GoogleId == user.GoogleId).FirstOrDefault();
                return newUser.Id;
            }
            return 0;
        }

        public void Save(User item)
        {
            _dbConnection.Database.Insert(item);
            _dbConnection.Database.Table<User>().Where(v => v.Login == item.Login && v.Password == item.Password).FirstOrDefault();
        }

        public void Delete(User item)
        {
            _dbConnection.Database.Delete(item);
        }

        public void Delete(int id)
        {
            var user = _dbConnection.Database.Table<User>().Where(v => v.Id == id).FirstOrDefault();
            _dbConnection.Database.Delete(user);
        }

        public User Get(int id)
        {
            return _dbConnection.Database.Table<User>().Where(v => v.Id == id).FirstOrDefault();
        }

        public void SaveRange(List<User> list)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(List<User> list)
        {
            throw new NotImplementedException();
        }

        public List<User> GetRange(int id)
        {
            throw new NotImplementedException();
        }
    }
}
