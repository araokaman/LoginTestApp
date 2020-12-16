using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LoginTestApp
{
    [JsonObject]
    public class JsonModel
    {
        [JsonProperty("access_token")]
        public string access_token_;
        [JsonProperty("expires_in")]
        public int expires_in_;
        [JsonProperty("refresh_token")]
        public string refresh_token_;
    }
}