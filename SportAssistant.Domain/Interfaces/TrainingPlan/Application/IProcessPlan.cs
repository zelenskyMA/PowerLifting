using SportAssistant.Domain.DbModels.TrainingPlan;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

public interface IProcessPlan
{
    /// <summary>
    /// Создание тренировочного плана с днями по шаблону
    /// </summary>
    /// <param name="templateId">Ид шаблона палана</param>
    /// <param name="creationDate">Дата начала тренировок</param>
    /// <param name="userId">Ид спортсмена, для которого создается план</param>
    /// <returns></returns>
    Task<int> AssignPlanAsync(int templateId, DateTime creationDate, int userId);

    /// <summary>
    /// Проверка допустимости создания / обновления плана текущим пользователем.
    /// </summary>
    /// <param name="userIdForCheck">Ид пользователя, которому принадлежит план</param>
    /// <returns></returns>
    Task<int> PlanningAllowedForUserAsync(int userIdForCheck);

    /// <summary>
    /// Проверка допустимости просмотра данных пользователя userIdForCheck текущим пользователем.
    /// </summary>
    /// <param name="userIdForCheck">Ид пользователя, которому принадлежит план</param>
    /// <returns></returns>
    Task<bool> ViewAllowedForDataOfUserAsync(int userIdForCheck);

    /// <summary>
    /// проверка количества активных планов. Не позволяем планировать слишком много.
    /// </summary>
    /// <param name="userId">Ид спортсмена</param>
    /// <returns></returns>
    Task CheckActivePlansLimitAsync(int userId);

    /// <summary>
    /// Получение планов, пересекающихся по датам с указанной датой для спортсмена
    /// </summary>
    /// <param name="creationDate">Дата нового плана</param>
    /// <param name="userId">Ид спортсмена</param>
    /// <param name="daysCount">Кол - во дней в плане</param>
    /// <returns></returns>
    Task<List<PlanDb>> GetCrossingPlansAsync(DateTime creationDate, int userId, int daysCount = 0);
}
