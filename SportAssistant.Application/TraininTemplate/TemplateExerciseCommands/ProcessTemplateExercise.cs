using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands
{
    public class ProcessTemplateExercise : IProcessTemplateExercise
    {
        private readonly IProcessTemplateExerciseSettings _processTemplateExerciseSettings;
        private readonly IProcessExercise _processExercise;
        private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;
        private readonly ITrainingCountersSetup _trainingCountersSetup;
        private readonly IContextProvider _contextProvider;
        private readonly IMapper _mapper;

        public ProcessTemplateExercise(
            IProcessTemplateExerciseSettings processTemplateExerciseSettings,
            IProcessExercise processExercise,
            ICrudRepo<TemplateExerciseDb> templateExerciseRepository,
            ITrainingCountersSetup trainingCountersSetup,
            IContextProvider contextProvider,
            IMapper mapper)
        {
            _processTemplateExerciseSettings = processTemplateExerciseSettings;
            _processExercise = processExercise;
            _templateExerciseRepository = templateExerciseRepository;
            _trainingCountersSetup = trainingCountersSetup;
            _contextProvider = contextProvider;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<TemplateExercise>> GetByDaysAsync(List<int> dayIds)
        {
            var templateExerciseDb = await _templateExerciseRepository.FindAsync(t => dayIds.Contains(t.TemplateDayId));
            var exercises = await PrepareExerciseDataAsync(templateExerciseDb);
            return exercises;
        }

        /// <inheritdoc />
        public async Task<List<TemplateExercise>> PrepareExerciseDataAsync(List<TemplateExerciseDb> templateExerciseDb)
        {
            if (templateExerciseDb.Count() == 0)
            {
                return new List<TemplateExercise>();
            }

            var exerciseIds = templateExerciseDb.Select(t => t.ExerciseId).Distinct().ToList();
            var exercises = await _processExercise.GetAsync(exerciseIds);

            var settings = await _processTemplateExerciseSettings.GetAsync(templateExerciseDb.Select(t => t.Id).ToList());

            var planExercises = templateExerciseDb.Select(t => _mapper.Map<TemplateExercise>(t)).ToList();
            foreach (var item in planExercises)
            {
                item.Exercise = exercises.First(t => t.Id == item.Exercise.Id).Clone();
                item.Exercise.PlannedExerciseId = item.Id;

                item.Settings = settings.Where(t => t.TemplateExerciseId == item.Id).OrderBy(t => t.WeightPercentage).ToList();

                _trainingCountersSetup.SetExerciseCounters(item);
            }

            return planExercises;
        }

        /// <inheritdoc />
        public async Task DeleteTemplateExercisesAsync(List<TemplateExerciseDb> templateExercises)
        {
            if (templateExercises.Count == 0)
            {
                return;
            }

            await _processTemplateExerciseSettings.DeleteByTemplateExerciseIdsAsync(templateExercises.Select(t => t.Id).ToList());
            _templateExerciseRepository.DeleteList(templateExercises);

            await _contextProvider.AcceptChangesAsync();
        }
    }
}
