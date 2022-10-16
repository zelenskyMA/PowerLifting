using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Repositories
{
    public interface IPlanExerciseSettingsRepository : ICrudRepo<PlanExerciseSettingsDb>
    {
        /// <summary>
        /// Get full list of persentages
        /// </summary>
        /// <returns></returns>
        Task<List<PercentageDb>> GetPercentagesAsync();
    }
}
