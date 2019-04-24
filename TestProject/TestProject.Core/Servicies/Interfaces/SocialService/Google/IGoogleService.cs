using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Servicies.Interfaces.SocialService.Google
{
    public interface IGoogleService
    {
        Task<User> GetGoogleUserAsync(string accessToken);
    }
}
