using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Interfaces.SocialService.Google;
using TestProject.Core.Models;

namespace TestProject.Core.Services
{
    public class GoogleService
        : IGoogleService
    {
        public async Task<String> GetEmailAsync(string tokenType, string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
            var json = await httpClient.GetStringAsync("https://www.googleapis.com/userinfo/email?alt=json");
            var email = JsonConvert.DeserializeObject<GoogleEmail>(json);
            return email.Data.Email;
        }

        public async Task<User> GetSocialNetworkAsync(string accessToken)
        {
            User account = new User();
            try
            {

                var httpClient = new HttpClient();
                var requestUrl =
                  "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token="
                  + accessToken;



                var json = await httpClient.GetStringAsync(requestUrl);


                var googleModel = JsonConvert.DeserializeObject<GoogleProfileModel>(json);
                var bytes = await httpClient.GetByteArrayAsync(googleModel.Picture.Url);
                account.ImagePath = Convert.ToBase64String(bytes);
                account.Login = googleModel.Name;
                account.GoogleId = googleModel.Id;
            }
            catch (Exception ex)
            {

            }

            return account;
        }


        public class GoogleEmail
        {
            public GoogleEmailData Data { get; set; }
        }

        public class GoogleEmailData
        {
            public string Email { get; set; }
        }
    }
}
