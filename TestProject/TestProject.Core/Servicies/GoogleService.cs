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
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Servicies.Interfaces.SocialService.Google;

namespace TestProject.Core.Servicies
{
    public class GoogleService
        : IGoogleService
    {
        private readonly IHttpHelper _httpHelper;

        private readonly IMvxLog _mvxLog;

        public GoogleService(IHttpHelper httpHelper, IMvxLog mvxLog)
        {
            _httpHelper = httpHelper;
            _mvxLog = mvxLog;
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
                _mvxLog.Trace(ex.Message);
            }

            return account;
        }
    }
}
