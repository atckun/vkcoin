using System;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    [Serializable]
    public class ShopResponse
    {
        [JsonProperty(propertyName: "response")]
        public int Response { get; set; }
    }
}