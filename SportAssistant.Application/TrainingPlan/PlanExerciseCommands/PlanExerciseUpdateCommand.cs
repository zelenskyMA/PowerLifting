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
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanExerciseUpdateCommand(
            IProcessPlan processPlan,
            IProcessPlanExerciseSettings processPlanExerciseSettings,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository)
        {
            _processPlan = processPlan;
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _planExerciseRepository = plannedExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var planExerciseDb = (await _planExerciseRepository.FindAsync(t => t.Id == param.PlanExercise.Id)).FirstOrDefault();
            if (planExerciseDb == null)
            {
                throw new BusinessException("Не найдено упражнение для обновления");
            }

            var userId = await _processPlan.PlanningAllowedForUserAsync(param.UserId);

            if (planExerciseDb.Comments != param.PlanExercise.Comments)
            {
                planExerciseDb.Comments = param.PlanExercise.Comments;
                _planExerciseRepository.Update(planExerciseDb);
            }

            await _processPlanExerciseSettings.UpdateAsync(userId, param.PlanExercise.Id, param.PlanExercise.Exercise.ExerciseTypeId, param.PlanExercise.Settings);

            return true;
        }

        public class Param
        {
            /// <summary>
            /// Пользователь, для которого идет планирование.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Упражнение, для которого планируются поднятия
            /// </summary>
            public PlanExercise PlanExercise { get; set; }
        }
    }
}
