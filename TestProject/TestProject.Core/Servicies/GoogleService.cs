using MvvmCross;
using MvvmCross.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Helpers;
using TestProject.Core.Models;
using TestProject.Core.Servicies.Interfacies.SocialService.Google;

namespace TestProject.Core.Servicies
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
                var reqrequestUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accessToken;
                var model = await httpHelper.Get<GoogleProfileModel>(reqrequestUrl);
                var image = await httpHelper.GetByte(model?.Picture?.Url);

                account.ImagePath = Convert.ToBase64String(image);
                account.Login = model?.Name;
                account.GoogleId = model?.Id;
            }
            catch (Exception ex)
            {
                var log = Mvx.IoCProvider.Resolve<IMvxLog>();
                log.Trace(ex.Message);
            }

            return account;
        }
    }
}
