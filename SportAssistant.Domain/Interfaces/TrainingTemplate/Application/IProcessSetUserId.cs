namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

public interface IProcessSetUserId
{
    /// <summary>
    /// Получение Ид спортсмена по Ид шаблоне
    /// </summary>
    /// <param name="id">Ид шаблона</param>
    /// <returns></returns>
    Task<int> GetByPlanId(int id);

    /// <summary>
    /// Получение Ид спортсмена по Ид дня в шаблоне
    /// </summary>
    /// <param name="id">Ид дня в шаблоне</param>
    /// <returns></returns>
    Task<int> GetByDayId(int id);

    /// <summary>
    /// Получение Ид спортсмена по Ид упражнения в шаблоне
    /// </summary>
    /// <param name="id">Ид запланированного упражнения в шаблоне</param>
    /// <returns></returns>
    Task<int> GetByPlanExerciseId(int id);
}
