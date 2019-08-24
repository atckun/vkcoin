using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VkCoin.Models
{
    [Serializable]
    public class ShopModel
    {
        [JsonProperty("response")]
        public int Response { get; set; }
    }
}