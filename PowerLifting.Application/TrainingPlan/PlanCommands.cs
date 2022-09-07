using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.Common;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanCommands : IPlanCommands
    {
        private readonly IPlanExerciseCommands _plannedExerciseCommands;

        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IMapper _mapper;

        public PlanCommands(
          IPlanExerciseCommands plannedExerciseCommands,
          ICrudRepo<PlanDb> trainingPlanRepository,
          ICrudRepo<PlanDayDb> trainingDayRepository,
          IMapper mapper)
        {
            _plannedExerciseCommands = plannedExerciseCommands;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Plan> GetPlanAsync(int Id)
        {
            var dbPlan = (await _trainingPlanRepository.FindAsync(t => t.Id == Id)).FirstOrDefault();
            if (dbPlan == null)
            {
                return null;
            }

            var plan = _mapper.Map<Plan>(dbPlan);

            var planDays = (await _trainingDayRepository.FindAsync(t => t.PlanId == dbPlan.Id)).Select(t => _mapper.Map<PlanDay>(t)).ToList();
            var planExercises = await _plannedExerciseCommands.GetAsync(planDays.Select(t => t.Id).ToList());
            foreach (var planDay in planDays)
            {
                planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
                SetPlanDayCounters(planDay);
            }

            plan.TrainingDays = planDays;

            return plan;
        }

        /// <inheritdoc />
        public async Task<PlanDay> GetPlanDayAsync(int dayId)
        {
            var planDaysDb = (await _trainingDayRepository.FindAsync(t => t.Id == dayId)).FirstOrDefault();
            if (planDaysDb == null)
            {
                return null;
            }

            var planExercises = await _plannedExerciseCommands.GetAsync(dayId);

            var planDay = _mapper.Map<PlanDay>(planDaysDb);
            planDay.Exercises = planExercises.Where(t => t.PlanDayId == dayId).OrderBy(t => t.Order).ToList();
            SetPlanDayCounters(planDay);

            return planDay;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(DateTime creationDate)
        {
            var plan = new PlanDb() { StartDate = creationDate, UserId = 1 };
            await _trainingPlanRepository.CreateAsync(plan);

            for (int i = 0; i < 7; i++) // 7 days standard plan
            {
                var trainingDay = new PlanDayDb() { PlanId = plan.Id, ActivityDate = creationDate.AddDays(i) };
                await _trainingDayRepository.CreateAsync(trainingDay);
            }

            return plan.Id;
        }

        /// <summary>
        /// Вычисляем необходимые суммы и справочные данные для отображения в интерфейсе.
        /// </summary>
        /// <param name="day"></param>
        private void SetPlanDayCounters(PlanDay day)
        {
            if (day.Exercises == null || day.Exercises.Count == 0)
            {
                return;
            }

            // простые суммы значений
            day.WeightLoadSum = day.Exercises.Sum(t => t.WeightLoad);
            day.LiftCounterSum = day.Exercises.Sum(t => t.LiftCounter);
            day.IntensitySum = day.Exercises.Sum(t => t.Intensity);

            // считаем, сколько упражнений по подтипам в тренировочном дне. 
            day.ExerciseTypeCounters = new List<NamedEntity>();
            var groups = day.Exercises.Select(t => t.Exercise).GroupBy(t => t.ExerciseSubTypeId);
            foreach (var item in groups)
            {
                day.ExerciseTypeCounters.Add(new NamedEntity()
                {
                    Id = item.Select(t => t.Id).First(),
                    Name = item.Select(t => t.ExerciseSubTypeName).First(),
                    Description = item.Count().ToString()
                });
            }

            // копируем первый сет значений и добавляем к нему все остальные по Ид процентовки.
            var listIntensities = day.Exercises.Select(t => t.LiftIntensities).ToList();
            var dayIntensities = listIntensities.First();
            listIntensities.RemoveAt(0);
            foreach (var itemList in listIntensities)
            {
                foreach (var item in itemList)
                {
                    var dayIntensityItem = dayIntensities.FirstOrDefault(t => t.Percentage.Id == item.Percentage.Id);
                    dayIntensityItem.Value += item.Value;
                }
            }

            day.LiftIntensities = dayIntensities;
        }
    }
}
