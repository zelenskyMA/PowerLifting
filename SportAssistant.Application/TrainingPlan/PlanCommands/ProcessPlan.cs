using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    public class ProcessPlan : IProcessPlan
    {
        private readonly IProcessUserInfo _processUserInfo;
        private readonly IProcessPlanDay _processPlanDay;
        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
        private readonly IContextProvider _provider;
        private readonly IUserProvider _user;

        public ProcessPlan(
            IProcessUserInfo processUserInfo,
            IProcessPlanDay processPlanDay,
            ICrudRepo<PlanDb> planRepository,
            ICrudRepo<TemplateDayDb> templateDayRepository,
            IContextProvider provider,
            IUserProvider user)
        {
            _processUserInfo = processUserInfo;
            _processPlanDay = processPlanDay;
            _planRepository = planRepository;
            _templateDayRepository = templateDayRepository;
            _provider = provider;
            _user = user;
        }

        /// <inheritdoc />
        public async Task<int> AssignPlanAsync(int templateId, DateTime creationDate, int userId)
        {
            var crossingPlansDb = await GetCrossingPlansAsync(creationDate, userId);
            foreach (var planDb in crossingPlansDb)
            {
                await _processPlanDay.DeleteByPlanIdAsync(planDb.Id);
                _planRepository.Delete(planDb);
                await _provider.AcceptChangesAsync();
            }
           
            var plan = new PlanDb() { StartDate = creationDate, UserId = userId };
            await _planRepository.CreateAsync(plan);
            await _provider.AcceptChangesAsync();

            var templateDays = (await _templateDayRepository.FindAsync(t => t.TemplatePlanId == templateId)).OrderBy(t => t.DayNumber).ToList();
            for (int i = 0; i < 7; i++) // 7 дней в плане. Завязано в ui
            {
                var dayId = await _processPlanDay.CreateAsync(userId, plan.Id, creationDate.AddDays(i), templateDays[i].Id);
            }

            return plan.Id;
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

        /// <inheritdoc />
        public async Task<List<PlanDb>> GetCrossingPlansAsync(DateTime creationDate, int userId)
        {
            var prevPlanDate = creationDate.AddDays(-6);
            var nextPlanDate = creationDate.AddDays(6);

            var crossingPlans = await _planRepository.FindAsync(t =>
                t.UserId == userId &&
                t.StartDate >= prevPlanDate &&
                t.StartDate <= nextPlanDate);

            return crossingPlans;
        }
    }
}
