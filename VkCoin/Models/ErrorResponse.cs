using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VkCoin.Models
{
    [Serializable]
    public class ErrorResponse
    {
        [JsonProperty("code")]
        public HttpStatusCode Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}