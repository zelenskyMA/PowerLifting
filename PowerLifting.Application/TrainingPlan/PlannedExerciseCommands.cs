using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlannedExerciseCommands : IPlannedExerciseCommands
    {
        private readonly IPlannedExerciseRepository _plannedExerciseRepository;
        private readonly IPercentageRepository _percentageRepository;
        private readonly ICrudRepo<ExercisePercentageDb> _exercisePercentageRepository;
        private readonly IExerciseSettingsRepository _exerciseSettingsRepository;
        private readonly IMapper _mapper;

        public PlannedExerciseCommands(
         IPlannedExerciseRepository plannedExerciseRepository,
         IPercentageRepository percentageRepository,
         ICrudRepo<ExercisePercentageDb> exercisePercentageRepository,
         IExerciseSettingsRepository exerciseSettingsRepository,
         IMapper mapper)
        {
            _plannedExerciseRepository = plannedExerciseRepository;
            _percentageRepository = percentageRepository;
            _exercisePercentageRepository = exercisePercentageRepository;
            _exerciseSettingsRepository = exerciseSettingsRepository;
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
                var exerciseId = exercises[i - 1].Id;

                await CreatePlannedExerciseWithSettings(trainingDayId, exerciseId, i);
            }
        }

        private async Task CreatePlannedExerciseWithSettings(int trainingDayId, int exerciseId, int order)
        {
            // create exercise for training day
            var exercise = new PlannedExerciseDb()
            {
                TrainingDayId = trainingDayId,
                ExerciseId = exerciseId,
                Order = order,
            };
            await _plannedExerciseRepository.CreateAsync(exercise);

            //set exercise settings for every percentage in planned exercise
            var percentages = (await _percentageRepository.GetAllAsync()).OrderBy(t => t.MinValue);
            foreach (var percentage in percentages)
            {
                var exerciseSettings = new ExerciseSettingsDb() { };
                await _exerciseSettingsRepository.CreateAsync(exerciseSettings);

                //connect all entities
                var exercisePercentage = new ExercisePercentageDb()
                {
                    PlannedExerciseId = exercise.Id,
                    ExerciseSettingsId = exerciseSettings.Id,
                    PercentageId = percentage.Id,
                };
                await _exercisePercentageRepository.CreateAsync(exercisePercentage);
            }
        }
    }
}
