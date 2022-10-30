using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TraininTemplate;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    public class ProcessPlanExercise : IProcessPlanExercise
    {
        private readonly IProcessPlanExerciseSettings _processPlanExerciseSettings;
        private readonly IProcessUserAchivements _processUserAchivements;
        private readonly IProcessExercise _processExercise;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly ICrudRepo<TemplateExerciseSettingsDb> _templateExerciseSettingsRepository;
        private readonly ITrainingCountersSetup _trainingCountersSetup;
        private readonly IContextProvider _provider;
        private readonly IMapper _mapper;

        public ProcessPlanExercise(
            IProcessPlanExerciseSettings processPlanExerciseSettings,
            IProcessUserAchivements processUserAchivements,
            IProcessExercise processExercise,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
            ICrudRepo<TemplateExerciseSettingsDb> templateExerciseSettingsRepository,
            ITrainingCountersSetup trainingCountersSetup,
            IContextProvider provider,
            IMapper mapper)
        {
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _processUserAchivements = processUserAchivements;
            _processExercise = processExercise;
            _planExerciseRepository = plannedExerciseRepository;
            _templateExerciseSettingsRepository = templateExerciseSettingsRepository;
            _trainingCountersSetup = trainingCountersSetup;
            _provider = provider;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetByDaysAsync(List<int> dayIds)
        {
            var planExerciseDb = await _planExerciseRepository.FindAsync(t => dayIds.Contains(t.PlanDayId));
            var exercises = await PrepareExerciseDataAsync(planExerciseDb);
            return exercises;
        }

        /// <inheritdoc />
        public async Task<List<PlanExercise>> PrepareExerciseDataAsync(List<PlanExerciseDb> planExercisesDb)
        {
            if (planExercisesDb.Count() == 0)
            {
                return new List<PlanExercise>();
            }

            var exerciseIds = planExercisesDb.Select(t => t.ExerciseId).Distinct().ToList();
            var exercises = await _processExercise.GetAsync(exerciseIds);

            var settings = await _processPlanExerciseSettings.GetAsync(planExercisesDb.Select(t => t.Id).ToList());

            var planExercises = planExercisesDb.Select(t => _mapper.Map<PlanExercise>(t)).ToList();
            foreach (var item in planExercises)
            {
                item.Exercise = exercises.First(t => t.Id == item.Exercise.Id).Clone();
                item.Exercise.PlannedExerciseId = item.Id;

                item.Settings = settings.Where(t => t.PlanExerciseId == item.Id).OrderBy(t => t.Weight).ToList();

                _trainingCountersSetup.SetExerciseCounters(item);
            }

            return planExercises;
        }

        /// <inheritdoc />
        public async Task CreateAsync(int userId, int dayId, int exerciseId, int order, TemplateExercise? templateExercise)
        {
            var planExercise = new PlanExerciseDb()
            {
                PlanDayId = dayId,
                ExerciseId = exerciseId,
                Order = order,
                Comments = templateExercise == null ? string.Empty : templateExercise.Comments,
            };

            await _planExerciseRepository.CreateAsync(planExercise);

            await AssignSettingsAsync(userId, planExercise, templateExercise);
        }

        /// <inheritdoc />
        public async Task DeletePlanExercisesAsync(List<PlanExerciseDb> planExercises)
        {
            if (planExercises.Count == 0)
            {
                return;
            }

            await _processPlanExerciseSettings.DeleteByPlanExerciseIdsAsync(planExercises.Select(t => t.Id).ToList());
            _planExerciseRepository.DeleteList(planExercises);

            await _provider.AcceptChangesAsync();
        }

        private async Task AssignSettingsAsync(int userId, PlanExerciseDb planExercise, TemplateExercise? templateExercise)
        {
            if (templateExercise == null)
            {
                return;
            }

            await _provider.AcceptChangesAsync();

            var exerciseTypeId = templateExercise.Exercise.ExerciseTypeId;
            var achivement = await _processUserAchivements.GetByExerciseTypeAsync(userId, exerciseTypeId);

            var templateExerciseSettingsDb = await _templateExerciseSettingsRepository.FindAsync(t => t.TemplateExerciseId == templateExercise.Id);
            List<PlanExerciseSettings> settings = templateExerciseSettingsDb.Select(t => new PlanExerciseSettings()
            {
                Weight = (t.WeightPercentage * achivement?.Result ?? 0) / 100,
                Iterations = t.Iterations,
                ExercisePart1 = t.ExercisePart1,
                ExercisePart2 = t.ExercisePart2,
                ExercisePart3 = t.ExercisePart3,
            }).ToList();

            await _processPlanExerciseSettings.UpdateAsync(userId, planExercise.Id, exerciseTypeId, settings);
        }
    }
}
