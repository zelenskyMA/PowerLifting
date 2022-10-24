using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessPlanExerciseSettings
    {
        /// <summary>
        /// Получение поднятий в упражнениях по списку Ид упражнений
        /// </summary>
        /// <param name="exerciseIds">Список Ид запланированных упражнений</param>
        /// <returns></returns>
        Task<List<PlanExerciseSettings>> GetAsync(List<int> exerciseIds);

        /// <summary>
        /// Обновление поднятий по списку в упражнении
        /// </summary>
        /// <param name="planExerciseId">Ид плана</param>
        /// <param name="achivement">Рекорд пользователя по ипу упражнения</param>
        /// <param name="settingsList">Список поднятий</param>
        /// <returns></returns>
        Task UpdateAsync(int planExerciseId, int achivement, List<PlanExerciseSettings> settingsList);

        /// <summary>
        /// Удаление поднятий в упражнениях из списка
        /// </summary>
        /// <param name="exerciseIds">Поднятия для удаления</param>
        /// <returns></returns>
        Task DeleteByPlanExerciseIdsAsync(List<int> exerciseIds);       
    }
}