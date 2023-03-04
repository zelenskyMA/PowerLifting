using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands
{
    /// <summary>
    /// Завершение упражнения в процентной колонке.
    /// </summary>
    public class PlanExerciseSettingsComplete : ICommand<PlanExerciseSettingsComplete.Param, bool>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanUserId _processPlanUserId;
        private readonly ICrudRepo<PlanExerciseSettingsDb> _exerciseSettingsRepository;

        public PlanExerciseSettingsComplete(
            IProcessPlan processPlan,
            IProcessPlanUserId processPlanUserId,
            ICrudRepo<PlanExerciseSettingsDb> exerciseSettingsRepository)
        {
            _processPlan = processPlan;
            _processPlanUserId = processPlanUserId;
            _exerciseSettingsRepository = exerciseSettingsRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var excercisesDb = await _exerciseSettingsRepository.FindAsync(t => param.Ids.Contains(t.Id));
            if (excercisesDb.Count == 0)
            {
                return false;
            }

            foreach (var item in excercisesDb)
            {
                var planUserId = await _processPlanUserId.GetByPlanExerciseSettingsId(item.Id);
                await _processPlan.PlanningAllowedForUserAsync(planUserId);

                item.Completed = true;
            }

            _exerciseSettingsRepository.UpdateList(excercisesDb);
            return true;
        }

        public class Param
        {
            public List<int> Ids { get; set; } = new List<int>();
        }
    }
}
