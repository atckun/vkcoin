﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using VkCoin.Abstractions;
using VkCoin.Enums;
using VkCoin.Flurl;
using VkCoin.Models;

namespace VkCoin
{
    public class VkCoin : IVkCoin
    {
        private readonly string _key;
        private readonly long _merchantId;

        private static readonly Random Random = new Random();

        public VkCoin(string key, long merchantId)
        {
            if (string.IsNullOrWhiteSpace(value: key))
                throw new ArgumentException(message: "Value cannot be null or whitespace.", paramName: nameof(key));
            if (merchantId <= 0) throw new ArgumentOutOfRangeException(paramName: nameof(merchantId));

            _key = key;
            _merchantId = merchantId;
        }

        private static async Task<T> SendApiRequestAsync<T>(
            string method,
            object @params,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            const string baseUrl = "https://coin-without-bugs.vkforms.ru/merchant/";
            var response = await baseUrl
                .AppendPathSegment(segment: method)
                .PostJsonAsync(data: @params, cancellationToken: cancellationToken)
                .ReceiveJson<T>()
                .ConfigureAwait(continueOnCapturedContext: false);

            return response;
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
                tx = new[] {@tx},
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
                throw new ArgumentException(message: "Value cannot be an empty collection.", paramName: nameof(@userIds));
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
            string url = default,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var @params = new
            {
                merchantId = _merchantId,
                key = _key,
                callback = url
            };
            
            var response = await SendApiRequestAsync<CallbackResponse>(
                    "set",
                    @params,
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Response;
        }
    }
}