using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    [Serializable]
    public class BalanceModel
    {
        [JsonProperty("response")]
        public IDictionary<string, string> Response { get; set; }
    }
}