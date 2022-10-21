using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessPlanDay
    {
        /// <summary>
        /// Получение запланированного дня по Ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanDay> GetAsync(int id);

        /// <summary>
        /// Удаление дней из плана по его идентификатору.
        /// </summary>
        /// <param name="planId">Ид тренировочного плана</param>
        /// <returns></returns>
        Task DeleteByPlanIdAsync(int planId);
    }
}
