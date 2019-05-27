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
        : IHttpHelper
    {

        public async Task<T> Get<T>(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync(url);

                    var result = JsonConvert.DeserializeObject<T>(json);

                    return result;
                } 
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public async Task<byte[]> GetByte(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var byteArray = await httpClient.GetByteArrayAsync(uri);

                return byteArray;
            }
        }
    }
}
