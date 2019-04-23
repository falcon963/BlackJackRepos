using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.Helpers
{
    public class HttpHelper
    {

        public HttpHelper()
        {
            
        }

        public async Task<T> Get<T>(string Url)
        {
            using(var _httpClient = new HttpClient())
            {
                var json = await _httpClient.GetStringAsync(Url);

                var getModel = JsonConvert.DeserializeObject<T>(json);
                return getModel;
            }
        }

        public async Task<byte[]> GetByte(string Uri)
        {
            using (var _httpClient = new HttpClient())
            {
                var getByte = await _httpClient.GetByteArrayAsync(Uri);
                return getByte;
            }
        }
    }
}
