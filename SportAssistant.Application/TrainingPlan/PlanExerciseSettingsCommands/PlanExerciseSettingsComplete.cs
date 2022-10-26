using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands
{
    /// <summary>
    /// Завершение упражнения в процентной колонке.
    /// </summary>
    public class PlanExerciseSettingsComplete : ICommand<PlanExerciseSettingsComplete.Param, bool>
    {
        private readonly ICrudRepo<PlanExerciseSettingsDb> _exerciseSettingsRepository;

        public PlanExerciseSettingsComplete(ICrudRepo<PlanExerciseSettingsDb> exerciseSettingsRepository)
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
