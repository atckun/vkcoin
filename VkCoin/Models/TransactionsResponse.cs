using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VkCoin.Models
{
    [Serializable]
    public class TransactionsResponse
    {
        [JsonProperty(propertyName: "id")] 
        public long Id { get; set; }

        [JsonProperty(propertyName: "from_id")]
        public long FromId { get; set; }

        [JsonProperty(propertyName: "to_id")]
        public long ToId { get; set; }

        [JsonProperty(propertyName: "amount")]
        public long Amount { get; set; }

        [JsonProperty(propertyName: "type")]
        public int Type { get; set; }

        [JsonProperty(propertyName: "payload")]
        public int Payload { get; set; }
        
        [JsonProperty(propertyName: "external_id")]
        public int ExternalId { get; set; }
        
        [JsonConverter(converterType: typeof(UnixDateTimeConverter))]
        [JsonProperty(propertyName: "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}