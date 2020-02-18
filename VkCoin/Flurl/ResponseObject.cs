using System;
using Newtonsoft.Json;

namespace VkCoin.Flurl
{
    [Serializable]
    public class ResponseObject<T> where T : class
    {
        [JsonProperty(propertyName: "response")]
        public T Response { get; set; }
    } 
}