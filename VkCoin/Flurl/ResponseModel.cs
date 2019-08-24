using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkCoin.Flurl
{
    [Serializable]
    public class ResponseEnumerable<T> where T : class
    {
        [JsonProperty("response")]
        public IEnumerable<T> Response { get; set; }
    }

    [Serializable]
    public class ResponseObject<T> where T : class
    {
        [JsonProperty("response")]
        public T Response { get; set; }
    } 
}