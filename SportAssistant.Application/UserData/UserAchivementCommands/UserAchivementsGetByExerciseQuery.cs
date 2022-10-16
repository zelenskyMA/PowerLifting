using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserAchivementCommands
{
    public class UserAchivementsGetByExerciseQuery : ICommand<UserAchivementsGetByExerciseQuery.Param, UserAchivement>
    {
        private readonly IProcessUserAchivements _processUserAchivements;

        public UserAchivementsGetByExerciseQuery(
            IProcessUserAchivements processUserAchivements)
        {
            _processUserAchivements = processUserAchivements;
        }

        /// <inheritdoc />
        public async Task<UserAchivement> ExecuteAsync(Param param)
        {
            var info = await _processUserAchivements.GetByExerciseTypeAsync(param.UserId, param.ExerciseTypeId);
            return info;
        }

        public class Param
        {
            public int UserId { get; set; }

            public int ExerciseTypeId { get; set; }
        }
    }
}
