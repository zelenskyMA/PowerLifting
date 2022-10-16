using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
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
