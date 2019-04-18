using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Helpers;
using TestProject.Core.Interfaces.SocialService.Google;
using TestProject.Core.Models;

namespace TestProject.Core.Services
{
    public class GoogleService
        : IGoogleService
    {

        public async Task<User> GetSocialNetworkAsync(string accessToken)
        {
            User account = new User();
            try
            {
                var httpHelper = new HttpHelper();
                var json = await httpHelper.Post<GoogleProfileModel>(accessToken, new GoogleProfileModel());
                var model = httpHelper.Deserialize<GoogleProfileModel>(json);
                var image = await httpHelper.GetByte(model.Picture.Url);

                account.ImagePath = Convert.ToBase64String(image);
                account.Login = model.Name;
                account.GoogleId = model.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
