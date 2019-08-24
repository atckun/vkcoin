using System;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    [Serializable]
    public class PaymentResponse
    {
        [JsonProperty(propertyName: "id")]
        public long Id { get; set; }
        
        [JsonProperty(propertyName: "amount")]
        public int Amount { get; set; }
        
        [JsonProperty(propertyName: "current")]
        public ulong Current { get; set; }
    }
}