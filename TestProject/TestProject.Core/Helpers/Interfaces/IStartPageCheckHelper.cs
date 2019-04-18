using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    public interface IStartPageCheckHelper
    {
        bool SocialNetworkLogin();
        bool AccountStatus();
        bool CheckAccountAccess();
        int SetUserId();
    }
}
