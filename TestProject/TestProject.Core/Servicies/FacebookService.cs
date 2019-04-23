using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using Newtonsoft.Json;
using TestProject.Core.Helpers;
using TestProject.Core.Servicies.Interfacies.SocialService.Facebook;
using MvvmCross.Logging;
using MvvmCross;

namespace TestProject.Core.Servicies
{
    public class FacebookService
        : IFacebookService
    {
        private readonly HttpHelper _httpHelper;

        private readonly IMvxLog _log;

        public FacebookService()
        {
            _httpHelper = new HttpHelper();

            _log = Mvx.IoCProvider.Resolve<IMvxLog>();
        }

        public async Task<User> GetFacebookUserAsync(string accessToken)
        {
            User account = new User();
            try
            {
                var reqrequestUrl = "https://graph.facebook.com/me?fields=email,id,name,picture&access_token=" + accessToken;
                var model = await _httpHelper.Get<FacebookProfileModel>(reqrequestUrl);
                if(model?.Picture?.Data?.Url != null)
                {
                    var image = await _httpHelper.GetByte(model?.Picture?.Data?.Url);
                    account.ImagePath = Convert.ToBase64String(image);
                    account.Login = model?.Name;
                    account.FacebookId = model?.Id;
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

