﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TestProject.Core.Helpers.Interfaces;

namespace TestProject.Core.Helpers
{
    public class HashHelper
        : IHashHelper
    {
        public HashHelper()
        {

        }

        public string HashPassword(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}