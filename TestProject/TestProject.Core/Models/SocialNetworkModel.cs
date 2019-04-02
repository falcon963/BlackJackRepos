using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Models
{
    public class SocialNetworkModel
    {
        public string UserID { get; set; }
        public string NetworkName { get; set; }
        public string AccessToken { get; set; }
        public int ScoreComponentA { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public int ScoreComponentB { get; set; }
        public string NUserName { get; set; }
        public string NLocation { get; set; }
        public string Data { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? TokenExpirationDate { get; set; }
    }
}
