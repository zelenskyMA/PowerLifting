using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Domain.Interfaces.Coaching.Application
{
    public interface IProcessTrainingRequest
    {
        /// <summary>
        /// Получение имени тренера
        /// </summary>
        /// <param name="userId">Ид тренера</param>
        /// <returns></returns>
        Task<string> GetCoachName(int userId);

        /// <summary>
        /// Получение заявки на обучение по Ид пользователя
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        Task<TrainingRequest> GetByUserAsync(int userId);

        /// <summary>
        /// Удаление заявки
        /// </summary>
        /// <param name="userId">Ид пользователя в заявке</param>
        /// <returns></returns>
        Task RemoveAsync(int userId);
    }
}
