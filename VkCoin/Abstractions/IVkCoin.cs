using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkCoin.Enums;
using VkCoin.Models;

namespace VkCoin.Abstractions
{
    public interface IVkCoin
    {
        /// <summary>
        /// Получет ссылку на оплату VK Coin.
        /// <b>Обратите внимание, что сумма указывается в тысячных долях.</b>
        /// В примере выше 1000 = 1 VK Coin. Т.е., для того, чтобы отправить 0,001 VK Coin, нужно указать 1.
        /// </summary>
        /// <param name="amount">Сумма перевода.</param>
        /// <param name="payload">Любое число от -2000000000 до 2000000000, вернется вам в списке транзаций. Если не указано, отправляется случайное число.</param>
        /// <param name="freeAmount">Может ли пользователь изменять сумму перевода. По умолчанию False.</param>
        /// <returns>https://vk.com/coin#mMERCHANTID_AMOUNT_PAYLOAD.</returns>
        string GetPaymentUrl(float amount, int? payload = null, bool freeAmount = false);

        Task<IEnumerable<TransactionsResponse>> GetTransactionsAsync(
            Tx @tx,
            int? @lastTx = null,
            CancellationToken cancellationToken = default);

        Task<PaymentResponse> SendPaymentAsync(
            int @toId,
            float @amount,
            bool @markAsMerchant = false,
            CancellationToken cancellationToken = default);

        Task<IDictionary<string, string>> GetBalanceAsync(CancellationToken cancellationToken = default);

        Task<IDictionary<string, string>> GetBalanceAsync(
            long[] @userIds,
            CancellationToken cancellationToken = default);

        Task<int> SetShopNameAsync(
            string @name,
            CancellationToken cancellationToken = default);

        Task<string> SetCallbackAsync(
            string url = default,
            CancellationToken cancellationToken = default);

        Task<string> DeleteCallbackAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> GetCallbackLogsAsync(CancellationToken cancellationToken = default);
    }
}