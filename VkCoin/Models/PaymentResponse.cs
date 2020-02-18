using System;
using Newtonsoft.Json;

namespace VkCoin.Models
{
    /// <summary>
    /// ����� VK Coin API �� ����� /send
    /// </summary>
    [Serializable]
    public class PaymentResponse
    {
        /// <summary>
        /// Id ��������
        /// </summary>
        [JsonProperty(propertyName: "id")]
        public long Id { get; set; }
        
        /// <summary>
        /// ����� ��������
        /// </summary>
        [JsonProperty(propertyName: "amount")]
        public int Amount { get; set; }
        
        /// <summary>
        /// ���������� ������
        /// </summary>
        [JsonProperty(propertyName: "current")]
        public ulong Current { get; set; }
    }
}