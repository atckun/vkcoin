using System;
using Newtonsoft.Json;

namespace VkCoin.Flurl
{
    [Serializable]
    public class ResponseError<T> where T : class
    {
        [JsonProperty("error")]
        public T Response { get; set; }
    }
}