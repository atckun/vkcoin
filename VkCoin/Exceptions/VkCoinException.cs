using System;
using System.Collections.Generic;
using System.Text;

namespace VkCoin.Exceptions
{

    [Serializable]
    public class VkCoinException : Exception
    {
        public VkCoinException() { }
        public VkCoinException(string message) : base(message) { }
        public VkCoinException(string message, Exception inner) : base(message, inner) { }
        protected VkCoinException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
