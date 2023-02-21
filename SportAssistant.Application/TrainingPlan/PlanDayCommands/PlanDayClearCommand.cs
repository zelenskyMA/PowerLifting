using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands
{
    /// <summary>
    /// Удаление упражнений из тренировочного дня.
    /// </summary>
    public class PlanDayClearCommand : ICommand<PlanDayClearCommand.Param, bool>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly IProcessPlanUserId _processPlanUserId;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanDayClearCommand(
            IProcessPlan processPlan,
            IProcessPlanExercise processPlanExercise,
            IProcessPlanUserId processPlanUserId,
            ICrudRepo<PlanExerciseDb> planExerciseRepository)
        {
            _processPlan = processPlan;
            _processPlanExercise = processPlanExercise;
            _processPlanUserId = processPlanUserId;
            _planExerciseRepository = planExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var userId = await _processPlanUserId.GetByDayId(param.Id);            
            await _processPlan.PlanningAllowedForUserAsync(userId);

            var planExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == param.Id);
            await _processPlanExercise.DeletePlanExercisesAsync(planExercisesDb);

            return true;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
