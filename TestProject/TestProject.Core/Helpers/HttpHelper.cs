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
        private string _requestUrl;

        private readonly HttpClient _httpClient;

        public HttpHelper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T> Get<T>(string Url)
        {
            var json = await _httpClient.GetStringAsync(Url);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<byte[]> GetByte(string imageUrl)
        {
            return await _httpClient.GetByteArrayAsync(imageUrl);
        }
    }
}
