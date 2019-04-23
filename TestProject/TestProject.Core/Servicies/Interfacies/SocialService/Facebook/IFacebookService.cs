using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Servicies.Interfacies.SocialService.Facebook
{
    public interface IFacebookService
    {
        Task<User> GetFacebookUserAsync(string accessToken);
    }
}
