using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    public interface IAccountCheckHelper
    {
        bool IsSocialNetworkLogin();
        bool IsAccountStatus();
        bool IsCheckAccountAccess();
        int SetUserId();
    }
}
