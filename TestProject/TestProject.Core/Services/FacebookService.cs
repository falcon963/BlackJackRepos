using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Interfaces.SocialService.Facebook;
using TestProject.Core.Models;
using Newtonsoft.Json;

namespace TestProject.Core.Services
{
    public class FacebookService
        : IFacebookService
    {

        public async Task<String> GetEmailAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email&access_token={accessToken}");
            var email = JsonConvert.DeserializeObject<FacebookProfileModel>(json);
            return email.Email;
        }


        public async Task<User> GetSocialNetworkAsync(string accessToken)
        {
            User account = new User();
            try
            {
               
                var httpClient = new HttpClient();
                var requestUrl =
                  "https://graph.facebook.com/me?fields=email,id,name,picture&access_token="
                  + accessToken;



                var json = await httpClient.GetStringAsync(requestUrl);


                var facebookModel = JsonConvert.DeserializeObject<FacebookProfileModel>(json);
                var bytes = await httpClient.GetByteArrayAsync(facebookModel.Picture.Data.Url);
                account.ImagePath = Convert.ToBase64String(bytes);
                account.Login = facebookModel.Name;
                account.FacebookId = facebookModel.Id;
            }
            catch (Exception ex)
            {

            }

            return account;
        }
    }
}

