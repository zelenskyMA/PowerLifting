using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Обновление упражнения в сете для тренировочного дня. Создание поднятий (plan exercise settings)
    /// </summary>
    public class PlanExerciseUpdateCommand : ICommand<PlanExerciseUpdateCommand.Param, bool>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanExerciseSettings _processPlanExerciseSettings;
        private readonly IProcessPlanUserId _processPlanUserId;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanExerciseUpdateCommand(
            IProcessPlan processPlan,
            IProcessPlanExerciseSettings processPlanExerciseSettings,
            IProcessPlanUserId processPlanUserId,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository)
        {
            _processPlan = processPlan;
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _processPlanUserId = processPlanUserId;
            _planExerciseRepository = plannedExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var planExerciseDb = (await _planExerciseRepository.FindAsync(t => t.Id == param.PlanExercise.Id)).FirstOrDefault();
            if (planExerciseDb == null)
            {
                throw new BusinessException("Не найдено упражнение для обновления");
            }

            var userId = await GetAndCheckUserId(planExerciseDb.Id);

            planExerciseDb.Comments = param.PlanExercise.Comments;
            planExerciseDb.ExtPlanData = param.PlanExercise.ExtPlanData;
            _planExerciseRepository.Update(planExerciseDb);

            await _processPlanExerciseSettings.UpdateAsync(userId, param.PlanExercise.Id, param.PlanExercise.Exercise.ExerciseTypeId, param.PlanExercise.Settings);

            return true;
        }

        private async Task<int> GetAndCheckUserId(int planExerciseId)
        {
            var userId = await _processPlanUserId.GetByPlanExerciseId(planExerciseId);

            return await _processPlan.PlanningAllowedForUserAsync(userId);
        }

        public class Param
        {
            /// <summary>
            /// Упражнение, для которого планируются поднятия
            /// </summary>
            public PlanExercise PlanExercise { get; set; }
        }
    }
}
