using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;

namespace PowerLifting.Application.TrainingPlan.PlanExerciseSettingsCommands
{
    /// <summary>
    /// Complete planned exercises in a single percentage span.
    /// </summary>
    public class PlanExerciseSettingsComplete : ICommand<PlanExerciseSettingsComplete.Param, bool>
    {
        private readonly IPlanExerciseSettingsRepository _exerciseSettingsRepository;

        public PlanExerciseSettingsComplete(IPlanExerciseSettingsRepository exerciseSettingsRepository)
        {
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
