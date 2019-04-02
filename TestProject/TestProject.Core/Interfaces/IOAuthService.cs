using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using Xamarin.Auth;

namespace TestProject.Core.Interfaces
{
    public interface IOAuthService
    {
        OAuth2Authenticator AndroidLoginFacebook();
        OAuth2Authenticator IOSLoginFacebook();
        OAuth2Authenticator AndroidLoginGoogle();
        OAuth2Authenticator IOSLoginGoogle();
    }
}
