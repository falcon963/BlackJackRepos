using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using Newtonsoft.Json;
using TestProject.Core.Helpers;
using TestProject.Core.Servicies.Interfacies.SocialService.Facebook;

namespace TestProject.Core.Servicies
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
                var reqrequestUrl = "https://graph.facebook.com/me?fields=email,id,name,picture&access_token=" + accessToken;
                var model = await httpHelper.Get<FacebookProfileModel>(reqrequestUrl);
                var image = await httpHelper.GetByte(model?.Picture?.Data?.Url);

                account.ImagePath = Convert.ToBase64String(image);
                account.Login = model?.Name;
                account.FacebookId = model?.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return account;
        }
    }
}

