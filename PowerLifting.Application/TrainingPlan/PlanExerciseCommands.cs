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

        private readonly ICrudRepo<PlanExerciseDb> _plannedExerciseRepository;
        private readonly IMapper _mapper;

        public PlanExerciseCommands(
         IPlanExerciseSettingsCommands planExerciseSettingsCommands,
         IExerciseCommands exerciseCommands,
         ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
         IMapper mapper)
        {
            _planExerciseSettingsCommands = planExerciseSettingsCommands;
            _exerciseCommands = exerciseCommands;

            _plannedExerciseRepository = plannedExerciseRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task CreateAsync(int trainingDayId, List<Exercise> exercises)
        {
            if (exercises.Count == 0)
            {
                return;
            }

            for (int i = 1; i <= exercises.Count; i++)
            {
                var planExercise = new PlanExerciseDb()
                {
                    PlanDayId = trainingDayId,
                    ExerciseId = exercises[i - 1].Id,
                    Order = i,
                };
                await _plannedExerciseRepository.CreateAsync(planExercise);

                await _planExerciseSettingsCommands.Create(planExercise.Id);
            }
        }

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetAsync(int dayId) => await GetAsync(new List<int>() { dayId });

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetAsync(List<int> dayIds)
        {
            var planExercisesDb = await _plannedExerciseRepository.FindAsync(t => dayIds.Contains(t.PlanDayId));

            var exerciseIds = planExercisesDb.Select(t => t.ExerciseId).Distinct().ToList();
            var exercises = await _exerciseCommands.GetAsync(exerciseIds);

            var settings = await _planExerciseSettingsCommands.GetAsync(planExercisesDb.Select(t => t.Id).ToList());

            var planExercises = planExercisesDb.Select(t => _mapper.Map<PlanExercise>(t)).ToList();
            foreach (var item in planExercises)
            {
                item.Exercise = exercises.First(t => t.Id == item.Exercise.Id);
                item.Settings = settings.Where(t => t.PlanExerciseId == item.Id).OrderBy(t => t.Percentage.MinValue).ToList();

                SetPlanExerciseCounters(item);
            }

            return planExercises;
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
