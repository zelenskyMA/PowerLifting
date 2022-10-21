using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands
{
    public class ProcessPlanDay : IProcessPlanDay
    {
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly IPlanCountersSetup _planCountersSetup;
        private readonly IContextProvider _contextProvider;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IMapper _mapper;

        public ProcessPlanDay(
            IProcessPlanExercise processPlanExercise,
            IPlanCountersSetup planCountersSetup,
            IContextProvider contextProvider,
            ICrudRepo<PlanDayDb> planDayRepository,
            ICrudRepo<PlanExerciseDb> planExerciseRepository,
            IMapper mapper)
        {
            _processPlanExercise = processPlanExercise;
            _planCountersSetup = planCountersSetup;
            _contextProvider = contextProvider;
            _planDayRepository = planDayRepository;
            _planExerciseRepository = planExerciseRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<PlanDay> GetAsync(int id)
        {
            var planDayDb = (await _planDayRepository.FindAsync(t => t.Id == id)).FirstOrDefault();
            if (planDayDb == null)
            {
                return null;
            }

            var planExercises = await _processPlanExercise.GetByDaysAsync(new List<int>() { id });
            var percentages = planExercises.Where(t => t.Settings != null)
                .SelectMany(t => t.Settings.Select(z => z.Percentage))
                .DistinctBy(t => t.Id);

            var planDay = _mapper.Map<PlanDay>(planDayDb);
            planDay.Exercises = planExercises.Where(t => t.PlanDayId == id).OrderBy(t => t.Order).ToList();
            planDay.Percentages = planExercises.Where(t => t.Settings != null)
                .SelectMany(t => t.Settings.Select(z => z.Percentage))
                .DistinctBy(t => t.Id)
                .OrderBy(t => t.MinValue)
                .ToList();

            _planCountersSetup.SetPlanDayCounters(planDay);

            return planDay;
        }

        /// <inheritdoc />
        public async Task DeleteByPlanIdAsync(int planId)
        {
            var planDaysDb = await _planDayRepository.FindAsync(t => t.PlanId == planId);
            if (planDaysDb.Count == 0)
            {
                return;
            }

            var dayIds = planDaysDb.Select(t => t.Id);
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => dayIds.Contains(t.PlanDayId));
            await _processPlanExercise.DeletePlanExercisesAsync(planExercisesDb);

            _planDayRepository.DeleteList(planDaysDb);

            await _contextProvider.AcceptChangesAsync();
        }
    }
}
