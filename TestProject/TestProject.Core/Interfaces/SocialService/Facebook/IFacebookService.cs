using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces.SocialService.Facebook
{
    public interface IFacebookService
    {
        Task<User> GetSocialNetworkAsync(string accessToken);
    }
}
