using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Models.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanExerciseSettingsCommands : IPlanExerciseSettingsCommands
    {
        private readonly IExerciseCommands _exerciseCommands;

        private readonly IPlanExerciseSettingsRepository _exerciseSettingsRepository;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IMapper _mapper;

        public PlanExerciseSettingsCommands(
            IExerciseCommands exerciseCommands,
         IPlanExerciseSettingsRepository exerciseSettingsRepository,
         ICrudRepo<PlanExerciseDb> planExerciseRepository,
         IMapper mapper)
        {
            _exerciseCommands = exerciseCommands;

            _planExerciseRepository = planExerciseRepository;
            _exerciseSettingsRepository = exerciseSettingsRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<PlanExerciseSettings> GetAsync(int id)
        {
            var settingsDb = (await _exerciseSettingsRepository.FindAsync(t => t.Id == id)).FirstOrDefault();
            if (settingsDb == null)
            {
                return null;
            }

            var planExercise = (await _planExerciseRepository.FindAsync(t => t.Id == settingsDb.PlanExerciseId)).First();
            var exercise = await _exerciseCommands.GetAsync(planExercise.ExerciseId);
            var percentages = await GetPercentagesAsync();

            var settings = _mapper.Map<PlanExerciseSettings>(settingsDb);
            settings.Percentage = percentages.First(p => p.Id == settingsDb.PercentageId);
            settings.Exercise = exercise;

            return settings;
        }

        /// <inheritdoc />
        public async Task<List<PlanExerciseSettings>> GetAsync(List<int> planExerciseIds)
        {
            var percentages = await GetPercentagesAsync();

            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => planExerciseIds.Contains(t.PlanExerciseId));
            var settings = settingsDb.Select(t => _mapper.Map<PlanExerciseSettings>(t)).ToList();

            foreach (var item in settings)
            {
                var percentageId = settingsDb.First(t => t.Id == item.Id).PercentageId;
                item.Percentage = percentages.First(p => p.Id == percentageId);
            }

            return settings;
        }

        public async Task<List<Percentage>> GetPercentagesAsync() =>
            (await _exerciseSettingsRepository.GetPercentagesAsync()).Select(t => _mapper.Map<Percentage>(t)).OrderBy(t => t.MinValue).ToList();

        /// <inheritdoc />
        public async Task CreateAsync(int planExerciseId)
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
        public async Task UpdateAsync(PlanExerciseSettings settings) =>
            await _exerciseSettingsRepository.UpdateAsync(_mapper.Map<PlanExerciseSettingsDb>(settings));

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
