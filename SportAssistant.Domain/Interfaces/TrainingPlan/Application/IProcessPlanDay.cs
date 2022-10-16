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
    }
}
