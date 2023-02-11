using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserAchivementCommands
{
    /// <summary>
    /// Получение рекорда по упражнению в плане.
    /// Используется для отображения на странице упражнения при назначании поднятий в UI.
    /// </summary>
    public class UserAchivementsGetByExerciseQuery : ICommand<UserAchivementsGetByExerciseQuery.Param, UserAchivement>
    {
        private readonly IProcessUserAchivements _processUserAchivements;
        private readonly IProcessPlanUserId _processPlanUserId;

        public UserAchivementsGetByExerciseQuery(
            IProcessUserAchivements processUserAchivements,
            IProcessPlanUserId processPlanUserId)
        {
            _processUserAchivements = processUserAchivements;
            _processPlanUserId = processPlanUserId;
        }

        /// <inheritdoc />
        public async Task<UserAchivement> ExecuteAsync(Param param)
        {
            var userId = await _processPlanUserId.GetByPlanExerciseId(param.PlanExerciseId);
            if (userId == 0)
            {
                throw new BusinessException("Упражнение не найдено, нельзя определить рекорд.");
            }

            var info = await _processUserAchivements.GetByExerciseTypeAsync(userId, param.ExerciseTypeId);
            return info;
        }

        public class Param
        {         
            /// <summary>
            /// Ид упражнения в плане. Используется для получения ид исполнителя плана.
            /// Это может быть не ид запрашивающего, когда  тренер назначает упражнения ученику по его рекордам.
            /// </summary>
            public int PlanExerciseId { get; set; }

            /// <summary>
            /// Ид типа упражнения. Используется для определения рекорда исполнителя.
            /// </summary>
            public int ExerciseTypeId { get; set; }
        }
    }
}
