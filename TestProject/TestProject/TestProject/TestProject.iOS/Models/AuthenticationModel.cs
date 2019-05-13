using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TestProject.iOS.Models
{
    public class AuthenticationModel
    {
        public Action GoogleOnAuthenticationCanceled { get; set; }
        public Action GoogleOnAuthenticationCompleted { get; set; }
        public Action GoogleOnAuthenticationFailed { get; set; }
        public Action FacebookOnAuthenticationCanceled { get; set; }
        public Action FacebookOnAuthenticationCompleted { get; set; }
        public Action FacebookOnAuthenticationFailed { get; set; }
        public Action FacebookInitialize { get; set; }
        public Action GoogleInitialize { get; set; }
        public Action DissmisController { get; set; }
    }
}