using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Helpers
{
    public class HttpHelper
    {
        private string RequestUrl { get; set; }

        private HttpClient HttpClient { get; set; }

        public HttpHelper()
        {
            HttpClient = new HttpClient();
        }

        public async Task<string> Post<T>(string accessToken, T type)
        {
            if(type is GoogleProfileModel)
            {
                RequestUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accessToken}";
            }

            if (type is FacebookProfileModel)
            {
                RequestUrl = "https://graph.facebook.com/me?fields=email,id,name,picture&access_token={accessToken}";
            }

            return await HttpClient.GetStringAsync(RequestUrl);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<byte[]> GetByte(string imageUrl)
        {
            return await HttpClient.GetByteArrayAsync(imageUrl);
        }
    }
}
