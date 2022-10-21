using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Получение тренировочного плана по Ид.
    /// </summary>
    public class PlanGetByIdQuery : ICommand<PlanGetByIdQuery.Param, Plan>
    {
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly IPlanCountersSetup _planCountersSetup;

        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly IMapper _mapper;

        public PlanGetByIdQuery(
            IProcessPlanExercise processPlanExercise,
            IPlanCountersSetup planCountersSetup,
            ICrudRepo<PlanDb> planRepository,
            ICrudRepo<PlanDayDb> planDayRepository,
            IMapper mapper)
        {
            _processPlanExercise = processPlanExercise;
            _planCountersSetup = planCountersSetup;
            _planRepository = planRepository;
            _planDayRepository = planDayRepository;
            _mapper = mapper;
        }

        public async Task<Plan> ExecuteAsync(Param param)
        {
            var dbPlan = (await _planRepository.FindAsync(t => t.Id == param.Id)).FirstOrDefault();
            if (dbPlan == null)
            {
                return new Plan();
            }

            var plan = _mapper.Map<Plan>(dbPlan);

            var planDays = (await _planDayRepository.FindAsync(t => t.PlanId == dbPlan.Id)).Select(t => _mapper.Map<PlanDay>(t)).ToList();
            var planExercises = await _processPlanExercise.GetByDaysAsync(planDays.Select(t => t.Id).ToList());
            foreach (var planDay in planDays)
            {
                planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
                _planCountersSetup.SetPlanDayCounters(planDay);
            }

            plan.TrainingDays = planDays;
            _planCountersSetup.SetPlanCounters(plan);

            return plan;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
