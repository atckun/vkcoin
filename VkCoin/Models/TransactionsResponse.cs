using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VkCoin.Models
{
    /// <summary>
    /// ���������� �� ������ ����������
    /// </summary>
    [Serializable]
    public class TransactionsResponse
    {
        /// <summary>
        /// Id ����������
        /// </summary>
        [JsonProperty(propertyName: "id")] 
        public long Id { get; set; }

        /// <summary>
        /// Id �����������
        /// </summary>
        [JsonProperty(propertyName: "from_id")]
        public long FromId { get; set; }

        /// <summary>
        /// Id ����������
        /// </summary>
        [JsonProperty(propertyName: "to_id")]
        public long ToId { get; set; }

        /// <summary>
        /// ����� �������� � �������� ����� (1000)
        /// </summary>
        [JsonProperty(propertyName: "amount")]
        public long Amount { get; set; }

        /// <summary>
        /// ��� ����������. 3 - �������, 4 - ���������
        /// </summary>
        [JsonProperty(propertyName: "type")]
        public int Type { get; set; }

        /// <summary>
        /// ����� �� -2000000000 �� 2000000000
        /// </summary>
        [JsonProperty(propertyName: "payload")]
        public int Payload { get; set; }

        [JsonProperty(propertyName: "external_id")]
        public int ExternalId { get; set; }
        
        /// <summary>
        /// �����, ����� ���� ������� ����������
        /// </summary>
        [JsonConverter(converterType: typeof(UnixDateTimeConverter))]
        [JsonProperty(propertyName: "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}