using System;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    [Serializable]
    public class CallbackResponse
    {
        [JsonProperty(propertyName: "response")]
        public string Response { get; set; }
    }
}