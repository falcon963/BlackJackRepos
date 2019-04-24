﻿using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using TestProject.Core.DBConnection;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.DBConnection.Interfacies;
using TestProject.Core.Helpers.Interfaces;
using MvvmCross;

namespace TestProject.Core.Repositories
{
    public class LoginRepository
        : BaseRepository<User>, ILoginRepository
    {
        private readonly IUserHelper _userHelper;

        public LoginRepository(IDatabaseConnectionService dbConnection, IUserHelper userHelper) : base(dbConnection)
        {
            _userHelper = userHelper;
        }

        public void SetLoginAndPassword(string login, string password)
        {
            _userHelper.UserLogin = login;
            _userHelper.UserPassword = password;
        }

        public User GetAppRegistrateUserAccount(string login, string password)
        {
            var user = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.Login == login && v.Password == password);

            return user;
        }

        public bool CheckValidLogin(string login)
        {

            User result = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.Login == login);

            return result == null;

        }

        public void ChangePassword(int userId, string password)
        {
            User user = _dbConnection.Database.Table<User>().FirstOrDefault(u => u.Id == userId);

            user.Password = password;

            _dbConnection.Database.Update(user);
        }
        public void ChangeImage(int userId, string imagePath)
        {
            User user = _dbConnection.Database.Table<User>().FirstOrDefault(u => u.Id == userId);

            user.ImagePath = imagePath;

            _dbConnection.Database.Update(user);
        }

        public int GetSocialAccountUserId(User user)
        {
            User createdUser = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.FacebookId == user.FacebookId || v.GoogleId == user.GoogleId);
            if(createdUser != null)
            {
                _userHelper.UserId = createdUser.Id;

                return createdUser.Id;
            }
            if(createdUser == null)
            {
                _dbConnection.Database.Insert(user);

                User newUser = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.FacebookId == user.FacebookId || v.GoogleId == user.GoogleId);

                _userHelper.UserId = newUser.Id;

                return newUser.Id;
            }
            return 0;
        }

        public User Get(int userId)
        {
            var user = _dbConnection.Database.Table<User>().FirstOrDefault(v => v.Id == userId);

            return user;
        }
    }
}
