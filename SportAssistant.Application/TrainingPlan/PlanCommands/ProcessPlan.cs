using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    public class ProcessPlan : IProcessPlan
    {
        private readonly IProcessUserInfo _processUserInfo;
        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly IContextProvider _provider;
        private readonly IUserProvider _user;

        public ProcessPlan(
            IProcessUserInfo processUserInfo,
            ICrudRepo<PlanDb> planRepository,
            ICrudRepo<PlanDayDb> planDayRepository,
            IContextProvider provider,
            IUserProvider user)
        {
            _processUserInfo = processUserInfo;
            _planRepository = planRepository;
            _planDayRepository = planDayRepository;
            _provider = provider;
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
