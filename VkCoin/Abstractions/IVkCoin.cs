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
        /// <returns>https://vk.com/coin#mMERCHANTID_AMOUNT_PAYLOAD</returns>
        string GetPaymentUrl(float amount, int? payload = null, bool freeAmount = false);

        /// <summary>
        /// Получение списка транзакций.
        /// </summary>
        /// <param name="lastTx">Если указать в параметре lastTx номер последней транзакции, то будут возвращены только транзакции после указанной (но не более 1000 и 100 соотвественно).</param>
        /// <returns>Список транзакций.</returns>
        Task<IEnumerable<TransactionsResponse>> GetTransactionsAsync(
            Tx @tx,
            int? @lastTx = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Перевод
        /// </summary>
        /// <param name="toId">Id получателя.</param>
        /// <param name="amount">Сумма перевода в тысячных долях (1000).</param>
        /// <param name="markAsMerchant">Если передать дополнительный параметр markAsMerchant, то перевод в истории будет помечен как от магазина. При этом будет использовано название магазина, а не имя вашего аккаунта.</param>
        Task<PaymentResponse> SendPaymentAsync(
            int @toId,
            float @amount,
            bool @markAsMerchant = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение своего баланса.
        /// </summary>
        /// <returns>Словарь пользователь:баланс .</returns>
        Task<IDictionary<string, string>> GetBalanceAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение баланса пользователей.
        /// </summary>
        /// <param name="userIds">Id пользователей, баланс которых необходимо получить.</param>
        /// <returns>Словарь пользователь:баланс .</returns>
        Task<IDictionary<string, string>> GetBalanceAsync(
            long[] @userIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Установка названия магазина.
        /// </summary>
        /// <param name="name">Название магазина.</param>
        /// <returns>Ответ VK Coin API.</returns>
        Task<int> SetShopNameAsync(
            string @name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Установка callback-сервера.
        /// </summary>
        /// <param name="callback">Адрес сервера.</param>
        /// <returns>ON/OFF</returns>
        Task<string> SetCallbackAsync(
            string url = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление callback-сервера.
        /// </summary>
        /// <returns>ON/OFF</returns>
        Task<string> DeleteCallbackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение логов неудавшихся запросов.
        /// </summary>
        /// <returns>Логи неудавшихся запросов.</returns>
        Task<IEnumerable<string>> GetCallbackLogsAsync(CancellationToken cancellationToken = default);
    }
}