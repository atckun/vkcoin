using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkCoin.Models;

namespace VkCoin.Abstractions
{
    public interface IVkCoin
    {
        string GetPaymentUrl(int amount, int? payload = null, bool free = false);

        Task<IEnumerable<TransactionsModel>> GetTransactionsAsync(
            int type = 2,
            CancellationToken cancellationToken = default);

        Task<PaymentModel> SendPaymentAsync(
            int to,
            int amountt,
            CancellationToken cancellationToken = default);

        Task<IDictionary<string, string>> GetBalanceAsync(CancellationToken cancellationToken = default);
        
        Task<IDictionary<string, string>> GetBalanceAsync(
            long[] usersIds,
            CancellationToken cancellationToken = default);

        Task<int> SetShopNameAsync(
            string shopName,
            CancellationToken cancellationToken = default);
    }
}