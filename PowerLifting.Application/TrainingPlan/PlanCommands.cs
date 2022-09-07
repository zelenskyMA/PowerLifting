using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanCommands : IPlanCommands
    {
        private readonly IPlanExerciseCommands _plannedExerciseCommands;

        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IMapper _mapper;

        public PlanCommands(
          IPlanExerciseCommands plannedExerciseCommands,
          ICrudRepo<PlanDb> trainingPlanRepository,
          ICrudRepo<PlanDayDb> trainingDayRepository,
          IMapper mapper)
        {
            _plannedExerciseCommands = plannedExerciseCommands;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Plan> GetAsync(int Id)
        {
            var request = await _trainingPlanRepository.FindAsync(t => t.Id == Id);
            var dbPlan = request.FirstOrDefault();
            if (dbPlan == null)
            {
                return null;
            }

            var planDaysDb = await _trainingDayRepository.FindAsync(t => t.PlanId == dbPlan.Id);
            var planExercises = await _plannedExerciseCommands.GetAsync(planDaysDb.Select(t => t.Id).ToList());

            var planDays = planDaysDb.Select(t => _mapper.Map<PlanDay>(t)).ToList();
            foreach (var item in planDays)
            {
                item.Exercises = planExercises.Where(t => t.PlanDayId == item.Id).OrderBy(t => t.Order).ToList();
            }

            var plan = _mapper.Map<Plan>(dbPlan);
            plan.TrainingDays = planDays;

            return plan;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(DateTime creationDate)
        {
            var plan = new PlanDb() { StartDate = creationDate, UserId = 1 };
            await _trainingPlanRepository.CreateAsync(plan);

            for (int i = 0; i < 7; i++) // 7 days standard plan
            {
                var trainingDay = new PlanDayDb() { PlanId = plan.Id, ActivityDate = creationDate.AddDays(i) };
                await _trainingDayRepository.CreateAsync(trainingDay);
            }

            return plan.Id;
        }

    }
}
