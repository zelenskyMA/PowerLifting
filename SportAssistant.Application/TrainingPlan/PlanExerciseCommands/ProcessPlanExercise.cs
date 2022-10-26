using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    public class ProcessPlanExercise : IProcessPlanExercise
    {
        private readonly IProcessPlanExerciseSettings _processPlanExerciseSettings;
        private readonly IProcessExercise _processExercise;
        private readonly IContextProvider _contextProvider;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly ITrainingCountersSetup _trainingCountersSetup;
        private readonly IMapper _mapper;

        public ProcessPlanExercise(
            IProcessPlanExerciseSettings processPlanExerciseSettings,
            IProcessExercise processExercise,
            IContextProvider contextProvider,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
            ITrainingCountersSetup trainingCountersSetup,
            IMapper mapper)
        {
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _processExercise = processExercise;
            _contextProvider = contextProvider;
            _planExerciseRepository = plannedExerciseRepository;
            _trainingCountersSetup = trainingCountersSetup;
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
        public async Task DeletePlanExercisesAsync(List<PlanExerciseDb> planExercises)
        {
            if (planExercises.Count == 0)
            {
                return;
            }

            await _processPlanExerciseSettings.DeleteByPlanExerciseIdsAsync(planExercises.Select(t => t.Id).ToList());
            _planExerciseRepository.DeleteList(planExercises);

            await _contextProvider.AcceptChangesAsync();
        }
    }
}
