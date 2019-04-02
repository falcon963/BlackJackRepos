using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Models
{
    public class GoogleProfileModel
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("family_name")]
        public String FamilyName { get; set; }
        [JsonProperty("given_name")]
        public String GivenName { get; set; }
        [JsonProperty("link")]
        public String Link { get; set; }
        [JsonProperty("picture")]
        public Data Picture { get; set; }
        [JsonProperty("gender")]
        public String Gender { get; set; }
    }
}
