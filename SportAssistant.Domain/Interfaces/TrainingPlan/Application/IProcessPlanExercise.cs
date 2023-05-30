using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

public interface IProcessPlanExercise
{
    /// <summary>
    /// Получение запланированных упражнений по Ид дней в плане.
    /// </summary>
    /// <param name="dayIds">Список Ид дней в планах</param>
    /// <param name="countOnlyCompleted">Учитывать в подсчете показателей только успешно завершенные Поднятия</param>
    /// <returns></returns>
    Task<List<PlanExercise>> GetByDaysAsync(List<int> dayIds, bool countOnlyCompleted = false);

    /// <summary>
    /// Создание нового упражнения в тренировочном дне
    /// </summary>
    /// <param name="userId">Ид целевого спортсмена</param>
    /// <param name="dayId">Ид дня</param>
    /// <param name="exerciseId">Ид упражнения</param>
    /// <param name="order">Порядковый номер</param>
    /// <param name="templateExercise">Шабон упражнения</param>
    /// <returns></returns>
    Task CreateAsync(int userId, int dayId, int exerciseId, int order, TemplateExercise? templateExercise = null);

    /// <summary>
    /// Обогащаем данными модель запланированного упражнения
    /// </summary>
    /// <param name="planExercisesDb">Данные по упражнению из БД</param>
    /// <param name="countOnlyCompleted">Учитывать в подсчете показателей только успешно завершенные Поднятия</param>
    /// <returns></returns>
    Task<List<PlanExercise>> PrepareExerciseDataAsync(List<PlanExerciseDb> planExercisesDb, bool countOnlyCompleted = false);

    /// <summary>
    /// Удаление запланированных упражнений вместе с поднятиями (exerciseSettings)
    /// </summary>
    /// <param name="planExercises">Список для удаления</param>
    /// <returns></returns>
    Task DeletePlanExercisesAsync(List<PlanExerciseDb> planExercises);
}
