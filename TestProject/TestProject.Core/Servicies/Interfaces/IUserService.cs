using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Servicies.Interfaces
{
    public interface IUserService
    {
        int LoginInSocialAccount(User user);
    }
}
