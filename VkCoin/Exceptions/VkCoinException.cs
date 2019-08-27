using System;
using System.Collections.Generic;
using System.Text;

namespace VkCoin.Exceptions
{

    [Serializable]
    public class VkCoinException : Exception
    {
        private const string defaultMessage = "VK Coin API вернул ошибку";
        private const string additional = "VK Coin API вернул ошибку:\n";

        public VkCoinException() : base(defaultMessage) { }
        public VkCoinException(string message) : base(additional + message) { }
        public VkCoinException(string message, Exception inner) : base(message, inner) { }
        protected VkCoinException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
