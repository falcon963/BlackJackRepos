using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Interfaces.SocialService.Facebook;
using TestProject.Core.Models;
using Newtonsoft.Json;
using TestProject.Core.Helpers;

namespace TestProject.Core.Services
{
    public class FacebookService
        : IFacebookService
    {

        public async Task<User> GetSocialNetworkAsync(string accessToken)
        {
            User account = new User();
            try
            {
                var httpHelper = new HttpHelper();
                var json = await httpHelper.Post<FacebookProfileModel>(accessToken, new FacebookProfileModel());
                var model = httpHelper.Deserialize<FacebookProfileModel>(json);
                var image = await httpHelper.GetByte(model.Picture.Data.Url);

                account.ImagePath = Convert.ToBase64String(image);
                account.Login = model.Name;
                account.FacebookId = model.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return account;
        }
    }
}

