using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    public class ProcessPlan : IProcessPlan
    {
        private readonly IProcessUserInfo _processUserInfo;
        private readonly IUserProvider _user;

        public ProcessPlan(
            IProcessUserInfo processUserInfo,
            IUserProvider user)
        {
            _processUserInfo = processUserInfo;
            _user = user;
        }

        /// <inheritdoc />
        public async Task<int> PlanningAllowedForUserAsync(int userIdForCheck)
        {
            if (userIdForCheck == 0 || userIdForCheck == _user.Id) // план для себя
            {
                return _user.Id;
            }

            var info = await _processUserInfo.GetInfo(userIdForCheck);
            if (info?.CoachId != _user.Id) // тренерский план спортсмену
            {
                throw new BusinessException("У вас нет права планировать тренировки данного пользователя");
            }

            return userIdForCheck;
        }
    }
}
