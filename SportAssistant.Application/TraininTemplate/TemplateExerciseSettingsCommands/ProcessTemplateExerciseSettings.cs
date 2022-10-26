using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TraininTemplate;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TraininTemplate.TemplateExerciseSettingsCommands
{
    public class ProcessTemplateExerciseSettings : IProcessTemplateExerciseSettings
    {
        private readonly ICrudRepo<PercentageDb> _precentageRepository;
        private readonly ICrudRepo<TemplateExerciseSettingsDb> _exerciseSettingsRepository;
        private readonly IContextProvider _contextProvider;
        private readonly IMapper _mapper;

        public ProcessTemplateExerciseSettings(
            ICrudRepo<PercentageDb> precentageRepository,
            ICrudRepo<TemplateExerciseSettingsDb> exerciseSettingsRepository,
            IContextProvider contextProvider,
            IMapper mapper)
        {
            _precentageRepository = precentageRepository;
            _exerciseSettingsRepository = exerciseSettingsRepository;
            _contextProvider = contextProvider;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<TemplateExerciseSettings>> GetAsync(List<int> exerciseIds)
        {
            var percentages = await GetPercentageListAsync();

            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => exerciseIds.Contains(t.TemplateExerciseId));
            var settings = settingsDb.Select(t => _mapper.Map<TemplateExerciseSettings>(t)).ToList();

            foreach (var item in settings)
            {
                var percentageId = settingsDb.First(t => t.Id == item.Id).PercentageId;
                item.Percentage = percentages.First(p => p.Id == percentageId);
            }

            return settings;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int templateExerciseId, List<TemplateExerciseSettings> settingsList)
        {
            var existingSettingsDb = await _exerciseSettingsRepository.FindAsync(t => t.TemplateExerciseId == templateExerciseId);
            if (existingSettingsDb.Count() == 0 && (settingsList == null || settingsList.Count == 0))
            {
                return;
            }

            var newIds = settingsList.Select(t => t.Id);
            _exerciseSettingsRepository.DeleteList(existingSettingsDb.Where(t => !newIds.Contains(t.Id)).ToList());

            var percentages = await GetPercentageListAsync();
            var settingsListDb = existingSettingsDb
                .Where(t => newIds.Contains(t.Id))
                .Union(settingsList.Where(t => t.Id == 0).Select(t => _mapper.Map<TemplateExerciseSettingsDb>(t)))
                .ToList();

            foreach (var item in settingsListDb)
            {
                var updatedSettings = settingsList.FirstOrDefault(t => t.Id != 0 && t.Id == item.Id);
                if (updatedSettings != null)
                {
                    item.WeightPercentage = updatedSettings.WeightPercentage;
                    item.Iterations = updatedSettings.Iterations;
                    item.ExercisePart1 = updatedSettings.ExercisePart1;
                    item.ExercisePart2 = updatedSettings.ExercisePart2;
                    item.ExercisePart3 = updatedSettings.ExercisePart3;
                }

                var result = item.WeightPercentage;
                var percentage = percentages.FirstOrDefault(t => t.MinValue < result && t.MaxValue > result);
                percentage ??= percentages.OrderByDescending(t => t.MaxValue).First();

                item.TemplateExerciseId = templateExerciseId;
                item.PercentageId = percentage.Id;
            }

            await _exerciseSettingsRepository.CreateListAsync(settingsListDb.Where(t => t.Id == 0).ToList());
            _exerciseSettingsRepository.UpdateList(settingsListDb.Where(t => t.Id != 0).ToList());
        }

        /// <inheritdoc />
        public async Task DeleteByTemplateExerciseIdsAsync(List<int> exerciseIds)
        {
            if (exerciseIds.Count == 0)
            {
                return;
            }

            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => exerciseIds.Contains(t.TemplateExerciseId));
            _exerciseSettingsRepository.DeleteList(settingsDb.Select(t => _mapper.Map<TemplateExerciseSettingsDb>(t)).ToList());

            await _contextProvider.AcceptChangesAsync();
        }

        /// <inheritdoc />
        private async Task<List<Percentage>> GetPercentageListAsync() => (await _precentageRepository.GetAllAsync())
            .Select(t => _mapper.Map<Percentage>(t)).OrderBy(t => t.MinValue).ToList();
    }
}
