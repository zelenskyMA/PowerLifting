namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

public interface IProcessSetUserId
{

    /// <summary>
    /// Получение Ид трененра по Ид тренировочного цикла
    /// </summary>
    /// <param name="id">Ид цикла</param>
    /// <returns></returns>
    Task<int> GetBySetId(int id);

    /// <summary>
    /// Получение Ид трененра по Ид шаблона
    /// </summary>
    /// <param name="id">Ид шаблона</param>
    /// <returns></returns>
    Task<int> GetByPlanId(int id);

    /// <summary>
    /// Получение Ид трененра по Ид дня в шаблоне
    /// </summary>
    /// <param name="id">Ид дня в шаблоне</param>
    /// <returns></returns>
    Task<int> GetByDayId(int id);

    /// <summary>
    /// Получение Ид трененра по Ид упражнения в шаблоне
    /// </summary>
    /// <param name="id">Ид запланированного упражнения в шаблоне</param>
    /// <returns></returns>
    Task<int> GetByPlanExerciseId(int id);
}
