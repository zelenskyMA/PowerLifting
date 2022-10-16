using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.PlanExerciseCommands
{
    public class ProcessPlanExercise : IProcessPlanExercise
    {
        private readonly IProcessPlanExerciseSettings _processPlanExerciseSettings;
        private readonly IProcessExercise _processExercise;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IPlanCountersSetup _planCountersSetup;
        private readonly IMapper _mapper;

        public ProcessPlanExercise(
            IProcessPlanExerciseSettings processPlanExerciseSettings,
            IProcessExercise processExercise,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
            IPlanCountersSetup planCountersSetup,
            IMapper mapper)
        {
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _processExercise = processExercise;
            _planExerciseRepository = plannedExerciseRepository;
            _planCountersSetup = planCountersSetup;
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

                item.Settings = settings.Where(t => t.PlanExerciseId == item.Id).OrderBy(t => t.Percentage.MinValue).ToList();

                _planCountersSetup.SetPlanExerciseCounters(item);
            }

            return planExercises;
        }
    }
}
