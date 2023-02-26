using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands
{
    /// <summary>
    /// Получение тренировочного дня из плана по Ид.
    /// </summary>
    public class PlanDayGetByIdQuery : ICommand<PlanDayGetByIdQuery.Param, PlanDay>
    {
        private readonly IProcessPlanDay _processPlanDay;
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanUserId _processPlanUserId;

        public PlanDayGetByIdQuery(
            IProcessPlanDay processPlanDay,
            IProcessPlan processPlan,
            IProcessPlanUserId processPlanUserId)
        {
            _processPlanDay = processPlanDay;
            _processPlan = processPlan;
            _processPlanUserId = processPlanUserId;
        }

        public async Task<PlanDay> ExecuteAsync(Param param)
        {
            var planDay = await _processPlanDay.GetAsync(param.Id);
            if (planDay != null)
            {
                var planUserId = await _processPlanUserId.GetByDayId(param.Id);
                await _processPlan.ViewAllowedForDataOfUserAsync(planUserId);
            }

            return planDay;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
