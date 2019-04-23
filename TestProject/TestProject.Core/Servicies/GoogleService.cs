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
        private readonly HttpHelper _httpHelper;

        private readonly IMvxLog _log;

        public GoogleService()
        {
                _httpHelper = new HttpHelper();
                _log = Mvx.IoCProvider.Resolve<IMvxLog>();
        }

        public async Task<User> GetGoogleUserAsync(string accessToken)
        {
            User account = new User();
            try
            {
                var reqrequestUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accessToken;
                var model = await _httpHelper.Get<GoogleProfileModel>(reqrequestUrl);
                var image = await _httpHelper.GetByte(model?.Picture?.Url);
                if (model?.Picture?.Url != null)
                {
                    account.ImagePath = Convert.ToBase64String(image);
                    account.Login = model?.Name;
                    account.GoogleId = model?.Id;
                }
            }
            catch (Exception ex)
            {
                _log.Trace(ex.Message);
            }

            return account;
        }
    }
}
