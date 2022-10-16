using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands
{
    /// <summary>
    /// Получение тренировочного дня из плана на текущий день для вызывающего пользователя.
    /// </summary>
    public class PlanDayGetCurrentQuery : ICommand<PlanDayGetCurrentQuery.Param, PlanDay>
    {
        private readonly IProcessPlanDay _processPlanDay;
        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IUserProvider _user;

        public PlanDayGetCurrentQuery(
            IProcessPlanDay processPlanDay,
            ICrudRepo<PlanDb> trainingPlanRepository,
            ICrudRepo<PlanDayDb> trainingDayRepository,
            IUserProvider user)
        {
            _processPlanDay = processPlanDay;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _user = user;
        }

        public async Task<PlanDay> ExecuteAsync(Param param)
        {
            var now = DateTime.Now.Date;
            var emptyDay = new PlanDay();

            var dbPlans = await _trainingPlanRepository.FindAsync(t =>
                t.UserId == _user.Id &&
                t.StartDate <= now && t.StartDate >= now.AddDays(-6));
            if (!dbPlans.Any())
            {
                return emptyDay;
            }

            var planId = dbPlans.First().Id;
            var planDayDb = (await _trainingDayRepository.FindAsync(t => t.PlanId == planId && t.ActivityDate.Date == now)).FirstOrDefault();
            if (planDayDb == null)
            {
                return emptyDay;
            }

            return await _processPlanDay.GetAsync(planDayDb.Id);
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
