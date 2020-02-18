using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    [Serializable]
    public class BalanceResponse
    {
        [JsonProperty(propertyName: "response")]
        public IDictionary<string, string> Response { get; set; }
    }
}