using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Создание нового тренировочного плана на неделю от указанной даты.
    /// </summary>
    public class PlanDeleteCommand : ICommand<PlanDeleteCommand.Param, bool>
    {
        private readonly IProcessPlanDay _processPlanDay;
        private readonly IProcessPlan _processPlan;
        private readonly ICrudRepo<PlanDb> _planRepository;

        public PlanDeleteCommand(
            IProcessPlanDay processPlanDay,
            IProcessPlan processPlan,
            ICrudRepo<PlanDb> planRepository)
        {
            _processPlanDay = processPlanDay;
            _processPlan = processPlan;
            _planRepository = planRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var dbPlan = (await _planRepository.FindAsync(t => t.Id == param.Id)).FirstOrDefault();
            if (dbPlan == null)
            {
                return true;
            }

            var userId = await _processPlan.PlanningAllowedForUserAsync(dbPlan.UserId);

            await _processPlanDay.DeleteDayByPlanIdAsync(param.Id);
            _planRepository.Delete(dbPlan);

            return true;
        }

        public class Param
        {
            /// <summary>
            /// Ид плана
            /// </summary>
            public int Id { get; set; }
        }
    }
}
