using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ComposableAsync;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RateLimiter;
using VkCoin.Abstractions;
using VkCoin.Enums;
using VkCoin.Flurl;
using VkCoin.Models;
using VkCoin.Exceptions;

namespace VkCoin
{
    public class VkCoin : IVkCoin
    {
        private readonly string _key;
        private readonly long _merchantId;

        private static readonly Random Random = new Random();

        //merchant url
        const string baseUrl = "https://coin-without-bugs.vkforms.ru/merchant/";

        private static readonly TimeLimiter timeConstraint = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(20));

        /// <summary>
        /// API для работы с VK Coin.
        /// </summary>
        /// <param name="key">Ключ к платежному API.</param>
        /// <param name="merchantId">Ваш id.</param>
        public VkCoin(string key, long merchantId)
        {
            if (string.IsNullOrWhiteSpace(value: key))
                throw new ArgumentException(message: "Value cannot be null or whitespace.", paramName: nameof(key));
            if (merchantId <= 0) throw new ArgumentOutOfRangeException(paramName: nameof(merchantId));

            _key = key;
            _merchantId = merchantId;
        }

        /// <summary>
        /// Отправка запроса к VK Coin API.
        /// </summary>
        /// <typeparam name="T">Модель ответа.</typeparam>
        /// <param name="method">Метод VK Coin API.</param>
        /// <param name="params">Тело запроса (будет преобразовано в JSON).</param>
        private static async Task<T> SendApiRequestAsync<T>(
            string method,
            object @params,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (method == "set")
                await timeConstraint; //ограничение по времени на использование метода /set

            var response = await baseUrl
                .AppendPathSegment(segment: method)
                .PostJsonAsync(data: @params, cancellationToken: cancellationToken)
                .ReceiveJson<JObject>()
                .ConfigureAwait(continueOnCapturedContext: false);

            if (response.ContainsKey("error"))
            {
                var json = response.ToObject<ResponseError<ErrorResponse>>();
                var msg = json.Response.Message;
                throw new VkCoinException(msg);
            }

            return response.ToObject<T>();
        }

        public string GetPaymentUrl(float amount, int? payload = null, bool freeAmount = false)
        {
            var paymentUrl =
                $"https://vk.com/coin#m{_merchantId}_{amount}_{payload ?? Random.Next(minValue: -2000000000, maxValue: 2000000000)}";

            if (!freeAmount) return paymentUrl;
            return paymentUrl + "_1";
        }

        public async Task<IEnumerable<TransactionsResponse>> GetTransactionsAsync(
            Tx @tx = Tx.Hundred,
            int? @lastTx = null,
            CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(enumType: typeof(Tx), value: tx))
                throw new InvalidEnumArgumentException(
                    argumentName: nameof(tx),
                    invalidValue: (int) tx,
                    enumClass: typeof(Tx));

            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                tx = new[] {@tx}, //"tx": [1 или 2]
                lastTx = @lastTx
            };

            var response = await SendApiRequestAsync<ResponseArray<TransactionsResponse>>(
                    method: "tx",
                    @params: @params,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<PaymentResponse> SendPaymentAsync(
            int @toId,
            float @amount,
            bool @markAsMerchant = false,
            CancellationToken cancellationToken = default)
        {
            if (@toId <= 0) throw new ArgumentOutOfRangeException(paramName: nameof(@toId));
            if (@amount <= 0) throw new ArgumentOutOfRangeException(paramName: nameof(@amount));

            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                toId = @toId,
                amount = @amount,
                markAsMerchant = @markAsMerchant
            };

            var response = await SendApiRequestAsync<ResponseObject<PaymentResponse>>(
                    method: "send",
                    @params: @params,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<IDictionary<string, string>> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                userIds = new[] {_merchantId}
            };

            var response = await SendApiRequestAsync<BalanceResponse>(
                    "score",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<IDictionary<string, string>> GetBalanceAsync(
            long[] @userIds,
            CancellationToken cancellationToken = default)
        {
            if (@userIds.Length == 0)
                throw new ArgumentException(message: "Value cannot be an empty collection.",
                    paramName: nameof(@userIds));
            if (@userIds.Length > 100)
                throw new ArgumentException(message: "Length cannot be greater than 100", paramName: nameof(@userIds));
            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                userIds = @userIds
            };

            var response = await SendApiRequestAsync<BalanceResponse>(
                    "score",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<int> SetShopNameAsync(
            string @name,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value: @name))
                throw new ArgumentException(message: "Value cannot be null or whitespace.", paramName: nameof(@name));
            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                name = @name,
            };

            var response = await SendApiRequestAsync<ShopResponse>(
                    "set",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<string> SetCallbackAsync(
            string @callback,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(callback))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(callback));
            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                callback = @callback
            };

            var response = await SendApiRequestAsync<CallbackResponse>(
                    "set",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<string> DeleteCallbackAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                callback = 0 //в JSON 0 = null
            };

            var response = await SendApiRequestAsync<CallbackResponse>(
                    "set",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<IEnumerable<string>> GetCallbackLogsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                status = 1
            };

            var response = await SendApiRequestAsync<ResponseArray<string>>(
                    "set",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }
    }
}