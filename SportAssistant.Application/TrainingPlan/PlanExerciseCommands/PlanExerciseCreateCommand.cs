using SportAssistant.Application.TrainingPlan.PlanCommands;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Планирование сета упражнений в рамках тренировочного дня.
    /// </summary>
    public class PlanExerciseCreateCommand : ICommand<PlanExerciseCreateCommand.Param, bool>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly IProcessPlanUserId _processPlanUserId;
        private readonly IProcessSettings _processSettings;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanExerciseCreateCommand(
            IProcessPlan processPlan,
            IProcessPlanExercise processPlanExercise,
            IProcessPlanUserId processPlanUserId,
            IProcessSettings processSettings,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository)
        {
            _processPlan = processPlan;
            _processPlanExercise = processPlanExercise;
            _processPlanUserId = processPlanUserId;
            _processSettings = processSettings;
            _planExerciseRepository = plannedExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            if (param.Exercises.Count == 0)
            {
                return false;
            }

            var settings = await _processSettings.GetAsync();
            if (param.Exercises.Count > settings.MaxExercises)
            {
                throw new BusinessException("Лимит упражнений в день превышен.");
            }

            var userId = await GetAndCheckUserId(param.DayId);

            var planExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == param.DayId);
            await RemoveDeletedExercisesAsync(planExercisesDb, param);

            for (int i = 1; i <= param.Exercises.Count; i++)
            {
                if (!UpdateExercise(planExercisesDb, param, i))
                {
                    await _processPlanExercise.CreateAsync(userId, param.DayId, param.Exercises[i - 1].Id, i);
                }
            }

            return true;
        }

        private async Task RemoveDeletedExercisesAsync(List<PlanExerciseDb> planExercisesDb, Param param)
        {
            var itemsToDelete = planExercisesDb.Where(t => !param.Exercises.Select(t => t.PlannedExerciseId).Contains(t.Id)).ToList();
            if (itemsToDelete.Count == 0)
            {
                return;
            }

            await _processPlanExercise.DeletePlanExercisesAsync(itemsToDelete);
            foreach (var item in itemsToDelete)
            {
                planExercisesDb.Remove(item);
            }
        }

        private bool UpdateExercise(List<PlanExerciseDb> planExercisesDb, Param param, int orderIndex)
        {
            var planExercise = planExercisesDb.FirstOrDefault(t => t.Id == param.Exercises[orderIndex - 1].PlannedExerciseId);
            if (planExercise != null)
            {
                planExercise.Order = orderIndex;
                _planExerciseRepository.Update(planExercise);
                return true;
            }

            return false;
        }

        private async Task<int> GetAndCheckUserId(int dayId)
        {
            var userId = await _processPlanUserId.GetByDayId(dayId);

            return await _processPlan.PlanningAllowedForUserAsync(userId);
        }

        public class Param
        {
            public int DayId { get; set; }

            public List<Exercise> Exercises { get; set; }
        }
    }
}