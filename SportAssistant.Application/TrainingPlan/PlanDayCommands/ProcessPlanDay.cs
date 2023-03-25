using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands
{
    public class ProcessPlanDay : IProcessPlanDay
    {
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly IProcessTemplateExercise _processTemplateExercise;
        private readonly ITrainingCountersSetup _trainingCountersSetup;
        private readonly IContextProvider _contextProvider;
        private readonly ICrudRepo<PlanDb> _planRepository;
        private readonly ICrudRepo<PlanDayDb> _planDayRepository;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IContextProvider _provider;
        private readonly IMapper _mapper;

        public ProcessPlanDay(
            IProcessPlanExercise processPlanExercise,
            IProcessTemplateExercise processTemplateExercise,
            ITrainingCountersSetup trainingCountersSetup,
            IContextProvider contextProvider,
            ICrudRepo<PlanDb> planRepository,
            ICrudRepo<PlanDayDb> planDayRepository,
            ICrudRepo<PlanExerciseDb> planExerciseRepository,
            IContextProvider provider,
            IMapper mapper)
        {
            _processPlanExercise = processPlanExercise;
            _trainingCountersSetup = trainingCountersSetup;
            _contextProvider = contextProvider;
            _planRepository = planRepository;
            _planDayRepository = planDayRepository;
            _planExerciseRepository = planExerciseRepository;
            _processTemplateExercise = processTemplateExercise;
            _provider = provider;
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

            _trainingCountersSetup.SetDayCounters(planDay);

            return planDay;
        }

        /// <inheritdoc />
        public async Task<PlanDay?> GetCurrentDay(int userId)
        {
            var now = DateTime.Now.Date;

            var dbPlans = await _planRepository.FindAsync(t =>
                t.UserId == userId &&
                t.StartDate.Date <= now && t.StartDate.Date >= now.AddDays(-6));
            if (!dbPlans.Any())
            {
                return null;
            }

            var planId = dbPlans.First().Id;
            var planDayDb = (await _planDayRepository.FindAsync(t => t.PlanId == planId && t.ActivityDate.Date == now)).FirstOrDefault();
            if (planDayDb == null)
            {
                return null;
            }

            return await GetAsync(planDayDb.Id);
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(int userId, int planId, DateTime activitydate, int templateDayId)
        {
            var trainingDay = new PlanDayDb() { PlanId = planId, ActivityDate = activitydate };
            await _planDayRepository.CreateAsync(trainingDay);

            if (templateDayId != 0)
            {
                await _provider.AcceptChangesAsync();
                var templateExercises = await _processTemplateExercise.GetByDaysAsync(new List<int>() { templateDayId });
                foreach (var templateExercise in templateExercises.OrderBy(t=> t.Order))
                {
                    await _processPlanExercise.CreateAsync(userId, trainingDay.Id, templateExercise.Exercise.Id, templateExercise.Order, templateExercise);
                }
            }

            return trainingDay.Id;
        }

        /// <inheritdoc />
        public async Task DeleteDayByPlanIdAsync(int planId)
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
