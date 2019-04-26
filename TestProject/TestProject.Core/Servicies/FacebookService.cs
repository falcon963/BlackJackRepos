using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using Newtonsoft.Json;
using TestProject.Core.Helpers;
using TestProject.Core.Servicies.Interfaces.SocialService.Facebook;
using MvvmCross.Logging;
using MvvmCross;
using TestProject.Core.Helpers.Interfaces;

namespace TestProject.Core.Servicies
{
    public class FacebookService
        : IFacebookService
    {
        private readonly IHttpHelper _httpHelper;

        private readonly IMvxLog _mvxLog;

        public FacebookService(IHttpHelper httpHelper, IMvxLog mvxLog)
        {
            _httpHelper = httpHelper;

            _mvxLog = mvxLog;
        }

        public async Task<User> GetUserAsync(string accessToken)
        {
            User account = new User();
            try
            {
                var requestUrl = "https://graph.facebook.com/me?fields=email,id,name,picture&access_token=" + accessToken;
                var model = await _httpHelper.Get<FacebookProfileModel>(requestUrl);
                string pictureUrl = model?.Picture?.Data?.Url;
                if (pictureUrl != null)
                {
                    var image = await _httpHelper.GetByte(pictureUrl);
                    account.ImagePath = Convert.ToBase64String(image);
                    account.Login = model?.Name;
                    account.FacebookId = model?.Id;
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

