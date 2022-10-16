using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Получение списка тренировочных планов по Ид пользователя.
    /// </summary>
    public class PlanGetListQuery : ICommand<PlanGetListQuery.Param, Plans>
    {
        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public PlanGetListQuery(
            ICrudRepo<PlanDb> trainingPlanRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _trainingPlanRepository = trainingPlanRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<Plans> ExecuteAsync(Param param)
        {
            var userId = param.UserId == 0 ? _user.Id : param.UserId;

            var plansDb = await _trainingPlanRepository.FindAsync(t => t.UserId == userId);
            var plansList = plansDb.Select(t => _mapper.Map<Plan>(t)).ToList();

            var plans = new Plans()
            {
                ActivePlans = plansList.Where(t => t.StartDate.AddDays(7) >= DateTime.Now.Date)
                    .OrderByDescending(t => t.StartDate).ToList(),

                ExpiredPlans = plansList.Where(t => t.StartDate.AddDays(7) < DateTime.Now.Date)
                    .OrderByDescending(t => t.StartDate).ToList(),
            };

            return plans;
        }

        public class Param
        {
            public int UserId { get; set; }
        }
    }
}
