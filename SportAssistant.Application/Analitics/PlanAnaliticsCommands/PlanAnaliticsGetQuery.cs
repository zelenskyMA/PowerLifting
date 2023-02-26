using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Analitics;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.Analitics.PlanAnaliticsCommands
{
    /// <summary>
    /// Подготовка и получение аналитики по тренировочным планам выбранного спортсмена
    /// </summary>
    public class PlanAnaliticsGetQuery : ICommand<PlanAnaliticsGetQuery.Param, PlanAnalitics>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly ITrainingCountersSetup _trainingCountersSetup;

        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public PlanAnaliticsGetQuery(
            IProcessPlan processPlan,
            IProcessPlanExercise processPlanExercise,
            ITrainingCountersSetup trainingCountersSetup,
            ICrudRepo<PlanDb> trainingPlanRepository,
            ICrudRepo<PlanDayDb> trainingDayRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _processPlan = processPlan;
            _processPlanExercise = processPlanExercise;
            _trainingCountersSetup = trainingCountersSetup;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<PlanAnalitics> ExecuteAsync(Param param)
        {
            await _processPlan.ViewAllowedForDataOfUserAsync(param.UserId);

            var plans = await PreparePlansWithCounters(param);
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

        private async Task<List<Plan>> PreparePlansWithCounters(Param param)
        {
            var userId = param.UserId == 0 ? _user.Id : param.UserId;

            // получаем планы, полностью попадающие в выбранный отрезок времени.
            var lastPlanDate = param.FinishDate.Date.AddDays(-6);
            var plans = (await _trainingPlanRepository.FindAsync(t => t.UserId == userId &&
                t.StartDate >= param.StartDate.Date && t.StartDate <= lastPlanDate))
                .Select(_mapper.Map<Plan>).ToList();

            // заполняем дто планов для подсчета аналитики
            var planIds = plans.Select(t => t.Id).ToList();
            var planDays = (await _trainingDayRepository.FindAsync(t => planIds.Contains(t.PlanId)))
                .Select(_mapper.Map<PlanDay>).ToList();

            var planExercises = await _processPlanExercise.GetByDaysAsync(planDays.Select(t => t.Id).ToList());

            foreach (var planDay in planDays)
            {
                planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
                _trainingCountersSetup.SetDayCounters(planDay);
            }

            foreach (var plan in plans)
            {
                plan.TrainingDays = planDays.Where(t => t.PlanId == plan.Id).ToList();
                _trainingCountersSetup.SetPlanCounters(plan);
            }

            return plans;
        }

        public class Param
        {
            public int UserId { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime FinishDate { get; set; }
        }
    }
}
