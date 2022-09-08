using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanExerciseSettingsCommands : IPlanExerciseSettingsCommands
    {
        private readonly IPlanExerciseSettingsRepository _exerciseSettingsRepository;
        private readonly IMapper _mapper;

        public PlanExerciseSettingsCommands(
         IPlanExerciseSettingsRepository exerciseSettingsRepository,
         IMapper mapper)
        {
            _exerciseSettingsRepository = exerciseSettingsRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PlanExerciseSettings>> GetAsync(int planExerciseId) => await GetAsync(new List<int>() { planExerciseId });

        /// <inheritdoc />
        public async Task<List<PlanExerciseSettings>> GetAsync(List<int> planExerciseIds)
        {
            var percentages = (await _exerciseSettingsRepository.GetPercentagesAsync()).Select(t => _mapper.Map<Percentage>(t)).ToList();

            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => planExerciseIds.Contains(t.PlanExerciseId));
            var settings = settingsDb.Select(t => _mapper.Map<PlanExerciseSettings>(t)).ToList();

            foreach (var item in settings)
            {
                var percentageId = settingsDb.First(t => t.Id == item.Id).PercentageId;
                item.Percentage = percentages.First(p => p.Id == percentageId);
            }

            return settings;
        }

        /// <inheritdoc />
        public async Task Create(int planExerciseId)
        {
            var percentages = (await _exerciseSettingsRepository.GetPercentagesAsync()).OrderBy(t => t.MinValue).ToList();

            foreach (var percentage in percentages)
            {
                var settings = new PlanExerciseSettingsDb()
                {
                    PercentageId = percentage.Id,
                    PlanExerciseId = planExerciseId,
                };

                await _exerciseSettingsRepository.CreateAsync(settings);
            }
        }

        /// <inheritdoc />
        public async Task DeleteByPlanExerciseIdAsync(List<int> planExerciseIds)
        {
            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => planExerciseIds.Contains(t.PlanExerciseId));
            foreach (var item in settingsDb)
            {
                await _exerciseSettingsRepository.DeleteAsync(_mapper.Map<PlanExerciseSettingsDb>(item));
            }
        }
    }
}
