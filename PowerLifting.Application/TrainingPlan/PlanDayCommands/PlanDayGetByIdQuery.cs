using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.PlanDayCommands
{
    /// <summary>
    /// Получение тренировочного дня из плана по Ид.
    /// </summary>
    public class PlanDayGetByIdQuery : ICommand<PlanDayGetByIdQuery.Param, PlanDay>
    {
        private readonly IProcessPlanDay _processPlanDay;

        public PlanDayGetByIdQuery(IProcessPlanDay processPlanDay)
        {
            _processPlanDay = processPlanDay;
        }

        public async Task<PlanDay> ExecuteAsync(Param param)
        {
            var planDay = await _processPlanDay.GetAsync(param.Id);
            return planDay;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
