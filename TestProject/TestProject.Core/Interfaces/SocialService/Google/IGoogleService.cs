using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces.SocialService.Google
{
    public interface IGoogleService
    {
        Task<String> GetEmailAsync(string tokenType, string accessToken);
        Task<User> GetSocialNetworkAsync(string accessToken);
    }
}
