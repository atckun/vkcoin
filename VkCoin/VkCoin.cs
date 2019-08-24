using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using VkCoin.Abstractions;
using VkCoin.Flurl;
using VkCoin.Models;

namespace VkCoin
{
    public class VkCoin : IVkCoin
    {
        private readonly string _key;
        private readonly long _merchantId;

        public VkCoin(string key, long merchantId)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            if (merchantId <= 0) throw new ArgumentOutOfRangeException(nameof(merchantId));

            _key = key;
            _merchantId = merchantId;
        }

        private static readonly Random  Random = new Random(); 
        
        public string GetPaymentUrl(int amount, int? payload = null, bool free = false)
        {
            var baseUrl = $"vk.com/coin#x{_merchantId}_{amount}_{payload ?? Random.Next(-2000000000, 2000000000)}";
            if (!free) return baseUrl;

            return baseUrl + "_1";
        }

        public async Task<IEnumerable<TransactionsModel>> GetTransactionsAsync(
            int type = 2,
            CancellationToken cancellationToken = default)
        {
            if (type <= 0) throw new ArgumentOutOfRangeException(nameof(type));
            cancellationToken.ThrowIfCancellationRequested();
            var response = await " https://coin-without-bugs.vkforms.ru/merchant/tx/".PostJsonAsync(new
                {
                    merchantId = _merchantId,
                    key = _key,
                    tx = new[] {type}
                }, cancellationToken: cancellationToken)
                .ReceiveJson<ResponseEnumerable<TransactionsModel>>()
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<PaymentModel> SendPaymentAsync(
            int to,
            int amountt,
            CancellationToken cancellationToken = default)
        {
            if (to <= 0) throw new ArgumentOutOfRangeException(nameof(to));
            if (amountt <= 0) throw new ArgumentOutOfRangeException(nameof(amountt));

            cancellationToken.ThrowIfCancellationRequested();
            var response = await "https://coin-without-bugs.vkforms.ru/merchant/send/".PostJsonAsync(new
                {
                    merchantId = _merchantId,
                    key = _key,
                    toId = to,
                    amount = amountt,
                }, cancellationToken: cancellationToken)
                .ReceiveJson<ResponseObject<PaymentModel>>()
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<IDictionary<string, string>> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var response = await "https://coin-without-bugs.vkforms.ru/merchant/score/".PostJsonAsync(new
            {
                merchantId = _merchantId,
                key = _key,
                userIds = new[] {_merchantId}
            }, cancellationToken: cancellationToken)
                .ReceiveJson<BalanceModel>()
                .ConfigureAwait(false);
            return response.Response;
        }
        
        public async Task<IDictionary<string, string>> GetBalanceAsync(
            long[] usersIds,
            CancellationToken cancellationToken = default)
        {
            if (usersIds.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(usersIds));
            if (usersIds.Length > 100)
                throw new ArgumentException("Length cannot be greater than 100", nameof(usersIds));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await "https://coin-without-bugs.vkforms.ru/merchant/score/".PostJsonAsync(new
                {
                    merchantId = _merchantId,
                    key = _key,
                    userIds = usersIds
                }, cancellationToken: cancellationToken)
                .ReceiveJson<BalanceModel>()
                .ConfigureAwait(false);

            return response.Response;
        }

        public async Task<int> SetShopNameAsync(
            string shopName,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(shopName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(shopName));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await "https://coin-without-bugs.vkforms.ru/merchant/set/".PostJsonAsync(new
                {
                    merchantId = _merchantId,
                    key = _key,
                    name = shopName,
                }, cancellationToken: cancellationToken)
                .ReceiveJson<ShopModel>()
                .ConfigureAwait(false);

            return response.Response;
        }
    }
}