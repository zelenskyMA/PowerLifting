using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
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
    }
}
