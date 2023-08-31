namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

public interface IProcessPlanUserId
{
    /// <summary>
    /// Получение Ид спортсмена по Ид дня в плане
    /// </summary>
    /// <param name="id">Ид тренировочного дня</param>
    /// <returns></returns>
    Task<int> GetByDayIdAsync(int id);

    /// <summary>
    /// Получение Ид спортсмена по Ид упражнения в плане
    /// </summary>
    /// <param name="id">Ид запланированного упражнения</param>
    /// <returns></returns>
    Task<int> GetByPlanExerciseIdAsync(int id);

    /// <summary>
    /// Получение Ид спортсмена по Ид поднятия в упражнении плана
    /// </summary>
    /// <param name="id">Ид запланированного упражнения</param>
    /// <returns></returns>
    Task<int> GetByPlanExerciseSettingsIdAsync(int id);
}
