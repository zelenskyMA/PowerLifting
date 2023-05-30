using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

public interface IProcessPlanDay
{
    /// <summary>
    /// Получение Ид текущего тренировочного дня для переданного пользователя
    /// </summary>
    /// <param name="userId">Ид спортсмена</param>
    /// <returns></returns>
    Task<PlanDay?> GetCurrentDay(int userId);

    /// <summary>
    /// Получение запланированного дня по Ид
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PlanDay> GetAsync(int id);

    /// <summary>
    /// Создание дня в тренировочном плане
    /// </summary>
    /// <param name="userId">Ид целевого спортсмена</param>
    /// <param name="planId">Ид плана</param>
    /// <param name="activitydate">Дата тренировочного дня</param>
    /// <param name="templateDayId">Ид дня в шаблоне для создание с последующим назначением</param>
    /// <returns>Ид нового дня</returns>
    Task<int> CreateAsync(int userId, int planId, DateTime activitydate, int templateDayId = 0);

    /// <summary>
    /// Удаление дней из плана по его идентификатору.
    /// </summary>
    /// <param name="planId">Ид тренировочного плана</param>
    /// <returns></returns>
    Task DeleteDayByPlanIdAsync(int planId);
}
