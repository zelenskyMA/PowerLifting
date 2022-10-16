using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands
{
    public class ProcessPlanDay : IProcessPlanDay
    {
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly IPlanCountersSetup _planCountersSetup;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly IMapper _mapper;

        public ProcessPlanDay(
            IProcessPlanExercise processPlanExercise,
            IPlanCountersSetup planCountersSetup,
            ICrudRepo<PlanDayDb> trainingDayRepository,
            IMapper mapper)
        {
            _processPlanExercise = processPlanExercise;
            _planCountersSetup = planCountersSetup;
            _planDayRepository = trainingDayRepository;
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
    }
}
