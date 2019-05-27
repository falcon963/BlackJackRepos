using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Models
{
    public class GoogleProfileModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string PictureUri { get; set; }

        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }
    }
}
