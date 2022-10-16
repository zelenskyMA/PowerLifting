using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Обновление упражнения в сете для тренировочного дня. Создание поднятий (plan exercise settings)
    /// </summary>
    public class PlanExerciseUpdateCommand : ICommand<PlanExerciseUpdateCommand.Param, bool>
    {
        private readonly IProcessPlanExerciseSettings _processPlanExerciseSettings;
        private readonly IProcessUserAchivements _processUserAchivements;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IUserProvider _user;

        public PlanExerciseUpdateCommand(
         IProcessPlanExerciseSettings processPlanExerciseSettings,
         IProcessUserAchivements processUserAchivements,
         ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
         IUserProvider user)
        {
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _processUserAchivements = processUserAchivements;
            _planExerciseRepository = plannedExerciseRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var planExerciseDb = (await _planExerciseRepository.FindAsync(t => t.Id == param.PlanExercise.Id)).FirstOrDefault();
            if (planExerciseDb == null)
            {
                throw new BusinessException("Не найдено упражнение для обновления");
            }

            if (planExerciseDb.Comments != param.PlanExercise.Comments)
            {
                planExerciseDb.Comments = param.PlanExercise.Comments;
                _planExerciseRepository.Update(planExerciseDb);
            }

            var userId = param.UserId == 0 ? _user.Id : param.UserId;
            var achivement = await _processUserAchivements.GetByExerciseTypeAsync(userId, param.PlanExercise.Exercise.ExerciseTypeId);
            if (achivement == null || achivement.Result == 0)
            {
                throw new BusinessException("Рекорд спортсмена не указан. Нельзя запланировать тренировку.");
            }

            await _processPlanExerciseSettings.UpdateAsync(param.PlanExercise.Id, achivement.Result, param.PlanExercise.Settings);

            return true;
        }

        public class Param
        {
            /// <summary>
            /// Пользователь, для которого идет планирование.
            /// </summary>
            public int UserId { get; set; }

            public PlanExercise PlanExercise { get; set; }
        }
    }
}
