using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces.SocialService.Facebook
{
    public interface IFacebookAuthentication
    {
        void OnAuthenticationCompleted(String token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}
