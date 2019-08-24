using System;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    [Serializable]
    public class PaymentModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("amount")]
        public int Amount { get; set; }
        
        [JsonProperty("current")]
        public ulong Current { get; set; }
    }
}