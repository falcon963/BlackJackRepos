using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.Servicies.Interfacies.SocialService.Facebook
{
    public interface IFacebookAuthentication
    {
        void OnAuthenticationCompleted(string token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}
