using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessPlanExercise
    {
        /// <summary>
        /// Получение запланированных упражнений по Ид дней в плане.
        /// </summary>
        /// <param name="dayIds">Список Ид дней в планах</param>
        /// <returns></returns>
        Task<List<PlanExercise>> GetByDaysAsync(List<int> dayIds);

        /// <summary>
        /// Обогащаем данными модель запланированного упражнения
        /// </summary>
        /// <param name="planExercisesDb">Данные по упражнению из БД</param>
        /// <returns></returns>
        Task<List<PlanExercise>> PrepareExerciseDataAsync(List<PlanExerciseDb> planExercisesDb);

        /// <summary>
        /// Удаление запланированных упражнений вместе с поднятиями (exerciseSettings)
        /// </summary>
        /// <param name="planExercises">Список для удаления</param>
        /// <returns></returns>
        Task DeletePlanExercisesAsync(List<PlanExerciseDb> planExercises);
    }
}
