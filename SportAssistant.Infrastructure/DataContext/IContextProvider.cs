using Microsoft.EntityFrameworkCore.Storage;

namespace SportAssistant.Infrastructure.DataContext
{
    public interface IContextProvider
    {
        /// <summary>
        /// Использовать только в BaseRepo. Все внешние действия только через доступный ф-цее класса
        /// </summary>
        SportContext Context { get; }

        /// <summary>
        /// Подтвердить часть изменений, сделанных до этого момента в БД.
        /// Транзакция не закрывается и доступен откат.
        /// </summary>
        /// <returns></returns>
        Task AcceptChangesAsync();

        /// <summary>
        /// Открыть новую транзакцию. Делается 1 раз на весь запрос к серверу
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Получить открытую транзакцию
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction GetTransaction();

        /// <summary>
        /// Откатить транзакцию в случае ошибки
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();

        /// <summary>
        /// подтвердить изменения и закрыть транзакцию
        /// </summary>
        /// <returns></returns>
        Task CommitTransactionAsync();
    }
}
