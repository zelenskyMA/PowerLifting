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

            // "распрямляем" данные по категориям упражнений.
            var typeIds = plans.SelectMany(t => t.Counters.CategoryCountersSum.Select(z => z.Id)).Distinct().ToList();
            foreach (var typeId in typeIds)
            {
                analitics.TypeCounters.Add(new CategoryCounterAnalitics()
                {
                    Id = typeId,
                    Name = plans.Select(t => t.Counters.CategoryCountersSum.FirstOrDefault(z => z.Id == typeId)).First(t => t?.Name != null).Name,
                    Values = new List<DateValueModel>()
                });
            }

            // готовим собранные данные к показу в инструменте графиков
            foreach (var plan in plans.OrderBy(t => t.StartDate).ToList())
            {
                var dataKey = plan.FinishDate.ToString("dd/MM/yy"); // ключ - дата, как одна из двух метрик

                analitics.PlanCounters.Add(new PlanCounterAnalitics()
                {
                    StartDate = plan.StartDate,
                    Name = dataKey,

                    //вычисляем суммы полученных данных по планам
                    LiftCounterSum = plan.TrainingDays.Sum(t => t.Counters.LiftCounterSum),
                    IntensitySum = plan.TrainingDays.Sum(t => t.Counters.IntensitySum),
                    WeightLoadSum = plan.TrainingDays.Sum(t => t.Counters.WeightLoadSum),
                });

                analitics.FullTypeCounterList.Add(new DateValueModel() { Name = dataKey });

                // готовим данные для графика по категориям упражнений
                foreach (var item in plan.Counters.CategoryCountersSum)
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

        /// <summary>
        /// Собираем данные и считаем метрики по тренировочным дням
        /// </summary>
        /// <param name="param">Период выборки данных</param>
        /// <returns></returns>
        private async Task<List<Plan>> PreparePlansWithCounters(Param param)
        {
            var userId = param.UserId == 0 ? _user.Id : param.UserId;

            // получаем планы, попадающие в выбранный отрезок времени. Для этого уменьшаем начало периода на длину одного плана
            var startPlanDate = param.StartDate.Date.AddDays(-6);
            var finishPlanDate = param.FinishDate.Date;
            var plans = (await _trainingPlanRepository.FindAsync(t => t.UserId == userId && 
                t.StartDate >= startPlanDate && t.StartDate <= finishPlanDate)).Select(_mapper.Map<Plan>).ToList();

            // получаем дни, четко попадающие в указанный период, лишние дни в планах не учитываем
            var planIds = plans.Select(t => t.Id).ToList();
            var planDays = (await _trainingDayRepository.FindAsync(t => planIds.Contains(t.PlanId) && 
                param.StartDate.Date <= t.ActivityDate && t.ActivityDate <= finishPlanDate)).Select(_mapper.Map<PlanDay>).ToList();

            // получаем упражнения и считаем статистику по ним. Записываем в дни
            var planExercises = await _processPlanExercise.GetByDaysAsync(planDays.Select(t => t.Id).ToList(), true); // отсекаем незавершенные поднятия
            foreach (var planDay in planDays)
            {
                planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
                _trainingCountersSetup.SetDayCounters(planDay);
            }

            // считаем статистику по дням и записываем их в планы
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
