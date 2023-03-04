using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Получение пользователя, которому принадлежит план.
    /// </summary>
    public class ProcessPlanUserId : IProcessPlanUserId
    {
        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly ICrudRepo<PlanExerciseSettingsDb> _exerciseSettingsRepository;

        public ProcessPlanUserId(
            ICrudRepo<PlanDb> planRepository,
            ICrudRepo<PlanDayDb> planDayRepository,
            ICrudRepo<PlanExerciseDb> planExerciseRepository,
            ICrudRepo<PlanExerciseSettingsDb> exerciseSettingsRepository)
        {
            _planRepository = planRepository;
            _planDayRepository = planDayRepository;
            _planExerciseRepository = planExerciseRepository;
            _exerciseSettingsRepository = exerciseSettingsRepository;
        }

        /// <inheritdoc />
        public async Task<int> GetByDayId(int id)
        {
            var day = await _planDayRepository.FindOneAsync(t => t.Id == id);
            if (day == null)
            {
                return 0;
            }

            var plan = await _planRepository.FindOneAsync(t => t.Id == day.PlanId);

            return plan?.UserId ?? 0;
        }

        /// <inheritdoc />
        public async Task<int> GetByPlanExerciseId(int id)
        {
            var planExercise = await _planExerciseRepository.FindOneAsync(t => t.Id == id);
            if (planExercise == null)
            {
                return 0;
            }

            return await GetByDayId(planExercise.PlanDayId);
        }

        /// <inheritdoc />
        public async Task<int> GetByPlanExerciseSettingsId(int id)
        {
            var planExerciseSettings = await _exerciseSettingsRepository.FindOneAsync(t => t.Id == id);
            if (planExerciseSettings == null)
            {
                return 0;
            }

            return await GetByPlanExerciseId(planExerciseSettings.PlanExerciseId);
        }
    }
}
