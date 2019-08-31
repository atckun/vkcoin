using System;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    /// <summary>
    /// Ответ VK Coin API на метод /send
    /// </summary>
    [Serializable]
    public class PaymentResponse
    {
        /// <summary>
        /// Id перевода
        /// </summary>
        [JsonProperty(propertyName: "id")]
        public long Id { get; set; }
        
        /// <summary>
        /// Сумма перевода
        /// </summary>
        [JsonProperty(propertyName: "amount")]
        public int Amount { get; set; }
        
        /// <summary>
        /// Оставшийся баланс
        /// </summary>
        [JsonProperty(propertyName: "current")]
        public ulong Current { get; set; }
    }
}