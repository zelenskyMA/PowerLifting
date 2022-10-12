using AutoMapper;
using PowerLifting.Domain.CustomExceptions;
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
                return new PlanExerciseSettings();
            }

            var planExercise = (await _planExerciseRepository.FindAsync(t => t.Id == settingsDb.PlanExerciseId)).First();
            var exercise = await _exerciseCommands.GetAsync(planExercise.ExerciseId);
            var percentages = await GetPercentageListAsync();

            var settings = _mapper.Map<PlanExerciseSettings>(settingsDb);
            settings.Percentage = percentages.First(p => p.Id == settingsDb.PercentageId);
            settings.Exercise = exercise;

            return settings;
        }

        /// <inheritdoc />
        public async Task<List<PlanExerciseSettings>> GetAsync(List<int> planExerciseIds)
        {
            var percentages = await GetPercentageListAsync();

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
        public async Task UpdateAsync(int planExerciseId, int achivement, List<PlanExerciseSettings> settingsList)
        {
            var existingSettingsDb = await _exerciseSettingsRepository.FindAsync(t => t.PlanExerciseId == planExerciseId);
            if (existingSettingsDb.Count() == 0 && (settingsList == null || settingsList.Count == 0))
            {
                return;
            }

            var newIds = settingsList.Select(t => t.Id);
            _exerciseSettingsRepository.DeleteList(existingSettingsDb.Where(t => !newIds.Contains(t.Id)).ToList());

            var percentages = await _exerciseSettingsRepository.GetPercentagesAsync();
            var settingsListDb = existingSettingsDb
                .Where(t => newIds.Contains(t.Id))
                .Union(settingsList.Where(t => t.Id == 0).Select(t => _mapper.Map<PlanExerciseSettingsDb>(t)))
                .ToList();

            foreach (var item in settingsListDb)
            {
                var updatedSettings = settingsList.FirstOrDefault(t => t.Id != 0 && t.Id == item.Id);
                if (updatedSettings != null)
                {
                    item.Weight = updatedSettings.Weight;
                    item.Iterations = updatedSettings.Iterations;
                    item.ExercisePart1 = updatedSettings.ExercisePart1;
                    item.ExercisePart2 = updatedSettings.ExercisePart2;
                    item.ExercisePart3 = updatedSettings.ExercisePart3;
                    item.Completed = false;
                }

                var result = item.Weight * 100 / achivement;
                var percentage = percentages.FirstOrDefault(t => t.MinValue < result && t.MaxValue > result);
                percentage ??= percentages.OrderByDescending(t => t.MaxValue).First();

                item.PlanExerciseId = planExerciseId;
                item.PercentageId = percentage.Id;
            }

            await _exerciseSettingsRepository.CreateListAsync(settingsListDb.Where(t => t.Id == 0).ToList());
            _exerciseSettingsRepository.UpdateList(settingsListDb.Where(t => t.Id != 0).ToList());
        }

        public async Task CompleteExercisesAsync(List<int> exerciseIds)
        {
            var excercisesDb = await _exerciseSettingsRepository.FindAsync(t => exerciseIds.Contains(t.Id));
            if (excercisesDb.Count == 0)
            {
                return;
            }

            foreach (var item in excercisesDb)
            {
                item.Completed = true;
            }

            _exerciseSettingsRepository.UpdateList(excercisesDb);
        }

        /// <inheritdoc />
        public async Task DeleteByPlanExerciseIdAsync(List<int> planExerciseIds)
        {
            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => planExerciseIds.Contains(t.PlanExerciseId));
            _exerciseSettingsRepository.DeleteList(settingsDb.Select(t => _mapper.Map<PlanExerciseSettingsDb>(t)).ToList());
        }

        /// <inheritdoc />
        public async Task<List<Percentage>> GetPercentageListAsync() => (await _exerciseSettingsRepository.GetPercentagesAsync())
            .Select(t => _mapper.Map<Percentage>(t)).OrderBy(t => t.MinValue).ToList();
    }
}
