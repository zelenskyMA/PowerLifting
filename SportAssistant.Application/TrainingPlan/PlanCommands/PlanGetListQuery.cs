using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Получение списка тренировочных планов по Ид пользователя.
    /// </summary>
    public class PlanGetListQuery : ICommand<PlanGetListQuery.Param, Plans>
    {
        private readonly IProcessPlan _processPlan;
        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public PlanGetListQuery(
            IProcessPlan processPlan,
            ICrudRepo<PlanDb> planRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _processPlan = processPlan;
            _planRepository = planRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<Plans> ExecuteAsync(Param param)
        {
            var userId = param.UserId == 0 ? _user.Id : param.UserId;

            await _processPlan.ViewAllowedForDataOfUserAsync(userId);

            var plansDb = await _planRepository.FindAsync(t => t.UserId == userId);
            var plansList = plansDb.Select(_mapper.Map<Plan>).ToList();

            var plans = new Plans()
            {
                ActivePlans = plansList.Where(t => t.StartDate.Date.AddDays(7) >= DateTime.Now.Date)
                    .OrderBy(t => t.StartDate).ToList(),

                ExpiredPlans = plansList.Where(t => t.StartDate.Date.AddDays(7) < DateTime.Now.Date)
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
