using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Analitics.Application;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application.Process;
using PowerLifting.Domain.Models.Analitics;
using PowerLifting.Domain.Models.Common;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.Analitics
{
    public class PlanAnaliticsCommands : IPlanAnaliticsCommands
    {
        private readonly IPlanExerciseCommands _plannedExerciseCommands;
        private readonly IPlanCountersSetup _planCountersSetup;

        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public PlanAnaliticsCommands(
            IPlanExerciseCommands plannedExerciseCommands,
            IPlanCountersSetup planCountersSetup,
            ICrudRepo<PlanDb> trainingPlanRepository,
            ICrudRepo<PlanDayDb> trainingDayRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _plannedExerciseCommands = plannedExerciseCommands;
            _planCountersSetup = planCountersSetup;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PlanDateAnalitics>> GetAsync(TimeSpanEntity span)
        {
            var plans = await PreparePlansWithCounters(span);
            var analitics = new List<PlanDateAnalitics>();
            if (plans.Count == 0)
            {
                return analitics;
            }

            foreach (var plan in plans.OrderBy(t => t.StartDate).ToList())
            {
                var analiticsItem = new PlanDateAnalitics()
                {
                    StartDate = plan.StartDate,
                    Name = plan.FinishDate.ToString("dd/MM/yy"),

                    LiftCounterSum = plan.TrainingDays.Sum(t => t.LiftCounterSum),
                    IntensitySum = plan.TrainingDays.Sum(t => t.IntensitySum),
                    WeightLoadSum = plan.TrainingDays.Sum(t => t.WeightLoadSum),
                };

                foreach (var item in plan.TypeCountersSum)
                {
                    switch (item.Id)
                    {
                        case 50: analiticsItem.ClassicJerk = item.Value; break;
                        case 51: analiticsItem.PushOnChest = item.Value; break;
                        case 52: analiticsItem.PushFromChest = item.Value; break;
                        case 53: analiticsItem.ClassicPush = item.Value; break;
                        case 54: analiticsItem.Ofp = item.Value; break;
                    }
                }

                analitics.Add(analiticsItem);
            }

            return analitics;
        }

        private async Task<List<Plan>> PreparePlansWithCounters(TimeSpanEntity span)
        {
            var lastPlanDate = span.FinishDate.Date.AddDays(-6);
            var plans = (await _trainingPlanRepository.FindAsync(t => t.UserId == _user.Id &&
                t.StartDate >= span.StartDate.Date && t.StartDate <= lastPlanDate))
                .Select(t => _mapper.Map<Plan>(t)).ToList();

            var planIds = plans.Select(t => t.Id).ToList();
            var planDays = (await _trainingDayRepository.FindAsync(t => planIds.Contains(t.PlanId)))
                .Select(t => _mapper.Map<PlanDay>(t)).ToList();

            var planExercises = await _plannedExerciseCommands.GetAsync(planDays.Select(t => t.Id).ToList());

            foreach (var planDay in planDays)
            {
                planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
                _planCountersSetup.SetPlanDayCounters(planDay);
            }

            foreach (var plan in plans)
            {
                plan.TrainingDays = planDays.Where(t => t.PlanId == plan.Id).ToList();
                _planCountersSetup.SetPlanCounters(plan);
            }

            return plans;
        }
    }
}
