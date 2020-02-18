namespace VkCoin.Enums
{
    /// <summary>
    /// Массив транзакций
    /// </summary>
    public enum Tx
    {
        /// <summary>
        /// 1000 последних транзакций со ссылок на оплату
        /// </summary>
        Thousand = 1,
        /// <summary>
        /// 100 последних транзакций на текущий аккаунт
        /// </summary>
        Hundred = 2,
    }
}