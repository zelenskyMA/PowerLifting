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
            var userId = await _processPlan.PlanningAllowedForUserAsync(param.UserId);

            var dbPlan = (await _planRepository.FindAsync(t => t.Id == param.Id && t.UserId == userId)).FirstOrDefault();
            if (dbPlan == null)
            {
                return true;
            }

            await _processPlanDay.DeleteByPlanIdAsync(param.Id);
            _planRepository.Delete(dbPlan);

            return true;
        }

        public class Param
        {
            /// <summary>
            /// Ид плана
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Ид пользователя, владельца плана
            /// </summary>
            public int UserId { get; set; }
        }
    }
}
