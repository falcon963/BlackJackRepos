using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core.Helpers.Interfaces
{
    public interface IHttpHelper
    {
        Task<T> Get<T>(string url);
        Task<byte[]> GetByte(string uri);
    }
}
