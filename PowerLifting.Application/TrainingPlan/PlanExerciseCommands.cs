using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanExerciseCommands : IPlanExerciseCommands
    {
        private readonly IPlanExerciseSettingsCommands _planExerciseSettingsCommands;
        private readonly IExerciseCommands _exerciseCommands;

        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IMapper _mapper;

        public PlanExerciseCommands(
         IPlanExerciseSettingsCommands planExerciseSettingsCommands,
         IExerciseCommands exerciseCommands,
         ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
         IMapper mapper)
        {
            _planExerciseSettingsCommands = planExerciseSettingsCommands;
            _exerciseCommands = exerciseCommands;

            _planExerciseRepository = plannedExerciseRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetAsync(int dayId) => await GetAsync(new List<int>() { dayId });

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetAsync(List<int> dayIds)
        {
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => dayIds.Contains(t.PlanDayId));

            var exerciseIds = planExercisesDb.Select(t => t.ExerciseId).Distinct().ToList();
            var exercises = await _exerciseCommands.GetAsync(exerciseIds);

            var settings = await _planExerciseSettingsCommands.GetAsync(planExercisesDb.Select(t => t.Id).ToList());

            var planExercises = planExercisesDb.Select(t => _mapper.Map<PlanExercise>(t)).ToList();
            foreach (var item in planExercises)
            {
                item.Exercise = exercises.First(t => t.Id == item.Exercise.Id).Clone();
                item.Exercise.PlannedExerciseId = item.Id;

                item.Settings = settings.Where(t => t.PlanExerciseId == item.Id).OrderBy(t => t.Percentage.MinValue).ToList();

                SetPlanExerciseCounters(item);
            }

            return planExercises;
        }

        /// <inheritdoc />
        public async Task CreateAsync(int dayId, List<Exercise> exercises)
        {
            if (exercises.Count == 0)
            {
                return;
            }

            //удаляем лишние записи вместе со связями
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == dayId);
            if (planExercisesDb.Count > 0)
            {
                var itemsToDelete = planExercisesDb.Where(t => !exercises.Select(t => t.PlannedExerciseId).Contains(t.Id)).ToList();
                if (itemsToDelete.Count > 0)
                {
                    await _planExerciseSettingsCommands.DeleteByPlanExerciseIdAsync(itemsToDelete.Select(t => t.Id).ToList());
                    await _planExerciseRepository.DeleteListAsync(itemsToDelete);

                    itemsToDelete.Select(t => planExercisesDb.Remove(t));
                }
            }

            for (int i = 1; i <= exercises.Count; i++)
            {
                // обновление существующего упражнения
                var planExercise = planExercisesDb.FirstOrDefault(t => t.Id == exercises[i - 1].PlannedExerciseId);
                if (planExercise != null)
                {
                    planExercise.Order = i;
                    await _planExerciseRepository.UpdateAsync(planExercise);
                    continue;
                }

                // добавление нового упражнения
                planExercise = new PlanExerciseDb()
                {
                    PlanDayId = dayId,
                    ExerciseId = exercises[i - 1].Id,
                    Order = i
                };

                await _planExerciseRepository.CreateAsync(planExercise);
                await _planExerciseSettingsCommands.Create(planExercise.Id);
            }
        }

        private void SetPlanExerciseCounters(PlanExercise planExercise)
        {
            if (planExercise.Settings == null || planExercise.Settings.Count == 0)
            {
                return;
            }

            planExercise.WeightLoad = planExercise.Settings.Select(
                t => t.Weight * t.Iterations * (t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3)).Sum();

            planExercise.LiftCounter = planExercise.Settings.Select(
                t => t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3).Sum();

            planExercise.Intensity = planExercise.LiftCounter == 0 ? 0 : planExercise.WeightLoad / planExercise.LiftCounter;

            planExercise.LiftIntensities = new List<LiftIntensity>();
            foreach (var item in planExercise.Settings)
            {
                planExercise.LiftIntensities.Add(new LiftIntensity()
                {
                    Percentage = item.Percentage,
                    Value = item.ExercisePart1 + item.ExercisePart2 + item.ExercisePart3,
                });
            };
        }
    }
}
