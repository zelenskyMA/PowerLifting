using AutoMapper;
using SportAssistant.Application.Settings;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands
{
    public class ProcessPlanExerciseSettings : IProcessPlanExerciseSettings
    {
        private readonly IProcessUserAchivements _processUserAchivements;
        private readonly IProcessSettings _processSettings;
        private readonly ICrudRepo<PercentageDb> _precentageRepository;
        private readonly ICrudRepo<PlanExerciseSettingsDb> _exerciseSettingsRepository;
        private readonly IContextProvider _contextProvider;
        private readonly IMapper _mapper;
        
        public ProcessPlanExerciseSettings(
            IProcessUserAchivements processUserAchivements,
            IProcessSettings processSettings,
            ICrudRepo<PercentageDb> precentageRepository,
            ICrudRepo<PlanExerciseSettingsDb> exerciseSettingsRepository,
            IContextProvider contextProvider,
            IMapper mapper)
        {
            _processUserAchivements = processUserAchivements;
            _processSettings = processSettings;
            _precentageRepository = precentageRepository;
            _exerciseSettingsRepository = exerciseSettingsRepository;
            _contextProvider = contextProvider;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PlanExerciseSettings>> GetAsync(List<int> exerciseIds)
        {
            var percentages = await GetPercentageListAsync();

            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => exerciseIds.Contains(t.PlanExerciseId));
            var settings = settingsDb.Select(t => _mapper.Map<PlanExerciseSettings>(t)).ToList();

            foreach (var item in settings)
            {
                var percentageId = settingsDb.First(t => t.Id == item.Id).PercentageId;
                item.Percentage = percentages.First(p => p.Id == percentageId);
            }

            return settings;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int userId, int planExerciseId, int exerciseTypeId, List<PlanExerciseSettings> settingsList)
        {
            var settings = await _processSettings.GetAsync();
            if (settingsList.Count > settings.MaxLiftItems)
            {
                throw new BusinessException("Лимит поднятий в упражнении превышен.");
            }

            var existingSettingsDb = await _exerciseSettingsRepository.FindAsync(t => t.PlanExerciseId == planExerciseId);
            if (existingSettingsDb.Count() == 0 && (settingsList == null || settingsList.Count == 0))
            {
                return;
            }

            var achivement = await _processUserAchivements.GetByExerciseTypeAsync(userId, exerciseTypeId);
            if (achivement == null || achivement.Result == 0)
            {
                throw new BusinessException("Рекорд спортсмена не указан. Нельзя запланировать тренировку.");
            }

            var newIds = settingsList.Select(t => t.Id);
            _exerciseSettingsRepository.DeleteList(existingSettingsDb.Where(t => !newIds.Contains(t.Id)).ToList());

            var percentages = await GetPercentageListAsync();
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
                    item.Completed = updatedSettings.Completed;
                }

                var result = item.Weight * 100 / achivement.Result;
                var percentage = percentages.FirstOrDefault(t => t.MinValue <= result && t.MaxValue >= result);
                percentage ??= percentages.OrderByDescending(t => t.MaxValue).First();

                item.PlanExerciseId = planExerciseId;
                item.PercentageId = percentage.Id;
            }

            await _exerciseSettingsRepository.CreateListAsync(settingsListDb.Where(t => t.Id == 0).ToList());
            _exerciseSettingsRepository.UpdateList(settingsListDb.Where(t => t.Id != 0).ToList());
        }

        /// <inheritdoc />
        public async Task DeleteByPlanExerciseIdsAsync(List<int> exerciseIds)
        {
            if (exerciseIds.Count == 0)
            {
                return;
            }

            var settingsDb = await _exerciseSettingsRepository.FindAsync(t => exerciseIds.Contains(t.PlanExerciseId));
            _exerciseSettingsRepository.DeleteList(settingsDb.Select(t => _mapper.Map<PlanExerciseSettingsDb>(t)).ToList());

            await _contextProvider.AcceptChangesAsync();
        }

        /// <inheritdoc />
        private async Task<List<Percentage>> GetPercentageListAsync() => (await _precentageRepository.GetAllAsync())
            .Select(t => _mapper.Map<Percentage>(t)).OrderBy(t => t.MinValue).ToList();
    }
}
