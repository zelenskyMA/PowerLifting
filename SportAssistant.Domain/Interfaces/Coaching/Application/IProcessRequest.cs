using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Domain.Interfaces.Coaching.Application;

public interface IProcessRequest
{
    /// <summary>
    /// Получение имени тренера
    /// </summary>
    /// <param name="userId">Ид тренера</param>
    /// <returns>Ид тренера из БД и его имя для отображения на UI</returns>
    Task<(int foundId, string name)> GetCoachName(int userId);

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
