using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Servicies.Interfaces.SocialService.Facebook
{
    public interface IFacebookService
    {
        Task<User> GetUserAsync(string accessToken);
    }
}
