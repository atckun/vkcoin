using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace VkCoin.Models
{
    [Serializable]
    public class TransactionsModel
    {
        [JsonProperty("id")] 
        public long Id { get; set; }

        [JsonProperty("from_id")]
        public long FromId { get; set; }

        [JsonProperty("to_id")]
        public long ToId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("payload")]
        public int Payload { get; set; }
        
        [JsonProperty("external_id")]
        public int ExternalId { get; set; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}