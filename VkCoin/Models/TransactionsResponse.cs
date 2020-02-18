using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VkCoin.Models
{
    /// <summary>
    /// Транзакция из списка транзакций
    /// </summary>
    [Serializable]
    public class TransactionsResponse
    {
        /// <summary>
        /// Id транзакции
        /// </summary>
        [JsonProperty(propertyName: "id")] 
        public long Id { get; set; }

        /// <summary>
        /// Id отправителя
        /// </summary>
        [JsonProperty(propertyName: "from_id")]
        public long FromId { get; set; }

        /// <summary>
        /// Id получателя
        /// </summary>
        [JsonProperty(propertyName: "to_id")]
        public long ToId { get; set; }

        /// <summary>
        /// Сумма перевода в тысячных долях (1000)
        /// </summary>
        [JsonProperty(propertyName: "amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Тип транзакции. 3 - перевод, 4 - получение
        /// </summary>
        [JsonProperty(propertyName: "type")]
        public int Type { get; set; }

        /// <summary>
        /// Число от -2000000000 до 2000000000
        /// </summary>
        [JsonProperty(propertyName: "payload")]
        public int Payload { get; set; }

        [JsonProperty(propertyName: "external_id")]
        public int ExternalId { get; set; }
        
        /// <summary>
        /// Время, когда была создана транзакция
        /// </summary>
        [JsonConverter(converterType: typeof(UnixDateTimeConverter))]
        [JsonProperty(propertyName: "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}