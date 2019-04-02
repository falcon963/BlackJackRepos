﻿using Plugin.SecureStorage;
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
    public class LoginService: ILoginService
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

        public Int32 CreateUser(User user)
        {
            _dbConnection.Database.Insert(user);
            User getingUser = _dbConnection.Database.Table<User>().Where(v => v.Login == user.Login && v.Password == user.Password).FirstOrDefault();
            if (getingUser == null)
            {
                return 0;
            }
            return getingUser.Id;
        }

        public User TakeProfile(Int32 userId)
        {
            //var users = _dbConnection.Database.Table<User>();
            User profile = _dbConnection.Database.Table<User>().Where(u => u.Id == userId).FirstOrDefault();
            return profile;
        }

        public void ChangePassword(Int32 userId, String password)
        {
            User user = _dbConnection.Database.Table<User>().Where(u => u.Id == userId).FirstOrDefault();
            user.Password = password;
            _dbConnection.Database.Update(user);
        }
        public void ChangeImage(Int32 userId, String imagePath)
        {
            User user = _dbConnection.Database.Table<User>().Where(u => u.Id == userId).FirstOrDefault();
            user.ImagePath = imagePath;
            _dbConnection.Database.Update(user);
        }

        public Int32 GetSocialAccount(User user)
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
    }
}
