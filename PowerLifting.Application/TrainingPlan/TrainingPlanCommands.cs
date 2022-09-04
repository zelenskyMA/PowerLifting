using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class TrainingPlanCommands : ITrainingPlanCommands
    {
        private readonly ITrainingPlanRepository _trainingPlanRepository;
        private readonly ITrainingDayRepository _trainingDayRepository;
        private readonly IMapper _mapper;

        public TrainingPlanCommands(
          ITrainingPlanRepository trainingPlanRepository,
          ITrainingDayRepository trainingDayRepository,
          IMapper mapper)
        {
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<TrainingPlanModel> GetAsync(int Id)
        {
            var dbPlan = (await _trainingPlanRepository.FindAsync(t => t.Id == Id)).FirstOrDefault();
            if (dbPlan == null)
            {
                return null;
            }

            var trainingDaysDb = await _trainingDayRepository.FindAsync(t => t.TrainingPlanId == dbPlan.Id);

            var plan = _mapper.Map<TrainingPlanModel>(dbPlan);
            foreach (var item in trainingDaysDb)
            {
                plan.TrainingDays.Add(_mapper.Map<TrainingDay>(item));
            }

            return plan;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(DateTime creationDate)
        {
            var plan = new TrainingPlanDb() { StartDate = creationDate, UserId = 1 };
            await _trainingPlanRepository.CreateAsync(plan);

            for (int i = 0; i < 7; i++) // 7 days standard plan
            {
                var trainingDay = new TrainingDayDb() { TrainingPlanId = plan.Id, ActivityDate = creationDate.AddDays(i) };
                await _trainingDayRepository.CreateAsync(trainingDay);
            }

            return plan.Id;
        }

    }
}
