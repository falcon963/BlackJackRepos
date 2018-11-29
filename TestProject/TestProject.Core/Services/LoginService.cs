using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.Services
{
    public class LoginService: ILoginService
    {

        public LoginService()
        {

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
    }
}
