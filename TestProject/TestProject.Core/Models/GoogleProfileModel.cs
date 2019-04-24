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

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("picture")]
        public Data Picture { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}
