using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Application.UserData.UserInfoCommands;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Создание нового тренировочного плана на неделю от указанной даты.
    /// </summary>
    public class PlanCreateCommand : ICommand<PlanCreateCommand.Param, int>
    {
        private readonly IProcessPlan _processPlan;
        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly IContextProvider _provider;

        public PlanCreateCommand(
            IProcessPlan processPlan,
            ICrudRepo<PlanDb> planRepository,
            ICrudRepo<PlanDayDb> planDayRepository,
            IContextProvider provider)
        {
            _processPlan = processPlan;
            _planRepository = planRepository;
            _planDayRepository = planDayRepository;
            _provider = provider;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            var userId = await _processPlan.PlanningAllowedForUserAsync(param.UserId);

            var prevPlanDate = param.CreationDate.AddDays(-6);
            var nextPlanDate = param.CreationDate.AddDays(6);
            var preventingPlans = await _planRepository.FindAsync(t =>
                t.UserId == userId &&
                t.StartDate >= prevPlanDate &&
                t.StartDate <= nextPlanDate);

            if (preventingPlans.Any())
            {
                string errorDates = string.Join(", ", preventingPlans.Select(t => t.StartDate.ToString("dd/MM/yyyy")));
                throw new BusinessException($"Найдены пересекающийся по датам планы. Даты начала: {errorDates}");
            }

            var plan = new PlanDb() { StartDate = param.CreationDate, UserId = userId };
            await _planRepository.CreateAsync(plan);
            await _provider.AcceptChangesAsync();

            for (int i = 0; i < 7; i++) // 7 days standard plan
            {
                var trainingDay = new PlanDayDb() { PlanId = plan.Id, ActivityDate = param.CreationDate.AddDays(i) };
                await _planDayRepository.CreateAsync(trainingDay);
            }

            return plan.Id;
        }

        public class Param
        {
            public DateTime CreationDate { get; set; }

            public int UserId { get; set; } = 0;
        }
    }
}
