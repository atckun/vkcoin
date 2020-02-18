using System;
using System.Collections.Generic;

namespace VkCoin.Flurl
{
    [Serializable]
    public class ResponseArray<T> : ResponseObject<IEnumerable<T>> where T : class
    {
    }
}