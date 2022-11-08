using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserAchivementCommands
{
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

            var info = await _processUserAchivements.GetByExerciseTypeAsync(userId, param.ExerciseTypeId);
            return info;
        }

        public class Param
        {
            public int PlanExerciseId { get; set; }

            public int ExerciseTypeId { get; set; }
        }
    }
}
