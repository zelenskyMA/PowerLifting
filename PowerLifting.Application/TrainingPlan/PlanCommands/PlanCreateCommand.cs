using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Infrastructure.Setup;

namespace PowerLifting.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Создание нового тренировочного плана на неделю от указанной даты.
    /// </summary>
    public class PlanCreateCommand : ICommand<PlanCreateCommand.Param, int>
    {
        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IContextProvider _provider;
        private readonly IUserProvider _user;

        public PlanCreateCommand(
            ICrudRepo<PlanDb> trainingPlanRepository,
            ICrudRepo<PlanDayDb> trainingDayRepository,
            IContextProvider provider,
            IUserProvider user)
        {
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _provider = provider;
            _user = user;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            var userId = _user.Id;
            if (param.UserId != 0)
            {
                // проверяем что _user.Id - тренер указанного UserId
                userId = param.UserId;
            }

            var prevPlanDate = param.CreationDate.AddDays(-6);
            var nextPlanDate = param.CreationDate.AddDays(6);
            var preventingPlans = await _trainingPlanRepository.FindAsync(t =>
                t.UserId == userId &&
                t.StartDate >= prevPlanDate &&
                t.StartDate <= nextPlanDate);

            if (preventingPlans.Any())
            {
                string errorDates = string.Join(", ", preventingPlans.Select(t => t.StartDate.ToString("dd/MM/yyyy")));
                throw new BusinessException($"Найдены пересекающийся по датам планы. Даты начала: {errorDates}");
            }

            var plan = new PlanDb() { StartDate = param.CreationDate, UserId = userId };
            await _trainingPlanRepository.CreateAsync(plan);
            await _provider.AcceptChangesAsync();

            for (int i = 0; i < 7; i++) // 7 days standard plan
            {
                var trainingDay = new PlanDayDb() { PlanId = plan.Id, ActivityDate = param.CreationDate.AddDays(i) };
                await _trainingDayRepository.CreateAsync(trainingDay);
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
