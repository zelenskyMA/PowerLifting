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
        public async Task<PlanAnalitics> GetAsync(int userId, TimeSpanEntity span)
        {
            var plans = await PreparePlansWithCounters(userId, span);
            var analitics = new PlanAnalitics();
            if (plans.Count == 0)
            {
                return analitics;
            }

            // "распрямляем" данные по подтипам упражнений.
            var typeIds = plans.SelectMany(t => t.TypeCountersSum.Select(z => z.Id)).Distinct().ToList();
            foreach (var typeId in typeIds)
            {
                analitics.TypeCounters.Add(new TypeCounterAnalitics()
                {
                    Id = typeId,
                    Name = plans.Select(t => t.TypeCountersSum.FirstOrDefault(z => z.Id == typeId)).First(t => t?.Name != null).Name,
                    Values = new List<DateValueModel>()
                });
            }

            // собираем плановые данные
            foreach (var plan in plans.OrderBy(t => t.StartDate).ToList())
            {
                var dataKey = plan.FinishDate.ToString("dd/MM/yy");

                analitics.PlanCounters.Add(new PlanCounterAnalitics()
                {
                    StartDate = plan.StartDate,
                    Name = dataKey,

                    LiftCounterSum = plan.TrainingDays.Sum(t => t.LiftCounterSum),
                    IntensitySum = plan.TrainingDays.Sum(t => t.IntensitySum),
                    WeightLoadSum = plan.TrainingDays.Sum(t => t.WeightLoadSum),
                });

                analitics.FullTypeCounterList.Add(new DateValueModel() { Name = dataKey });

                foreach (var item in plan.TypeCountersSum)
                {
                    var currentTypeCounter = analitics.TypeCounters.First(t => t.Id == item.Id);
                    currentTypeCounter.Values.Add(new DateValueModel()
                    {
                        Name = dataKey,
                        Value = item.Value
                    });
                }
            }

            return analitics;
        }

        private async Task<List<Plan>> PreparePlansWithCounters(int userId, TimeSpanEntity span)
        {
            userId = userId == 0 ? _user.Id : userId;

            var lastPlanDate = span.FinishDate.Date.AddDays(-6);
            var plans = (await _trainingPlanRepository.FindAsync(t => t.UserId == userId &&
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
