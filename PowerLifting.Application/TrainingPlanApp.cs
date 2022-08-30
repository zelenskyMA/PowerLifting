using AutoMapper;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Application
{
  public class TrainingPlanApp : ITrainingPlanApp
  {
    private readonly ITrainingPlanRepository _trainingPlanRepository;
    private readonly ITrainingDayRepository _trainingDayRepository;
    private readonly IMapper _mapper;

    public TrainingPlanApp(
      ITrainingPlanRepository trainingPlanRepository,
      ITrainingDayRepository trainingDayRepository,
      IMapper mapper)
    {
      _trainingPlanRepository = trainingPlanRepository;
      _trainingDayRepository = trainingDayRepository;
      _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TrainingPlan> GetAsync(int Id)
    {
      var dbPlan = (await _trainingPlanRepository.FindAsync(t => t.Id == Id)).FirstOrDefault();
      if (dbPlan == null)
      {
        return null;
      }

      var trainingDaysDb = await _trainingDayRepository.FindAsync(t => t.TrainingPlanId == dbPlan.Id);

      var plan = _mapper.Map<TrainingPlan>(dbPlan);
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
      await _trainingPlanRepository.Create(plan);

      for (int i = 0; i < 7; i++) // 7 days standard plan
      {
        var trainingDay = new TrainingDayDb() { TrainingPlanId = plan.Id, ActivityDate = creationDate.AddDays(i) };
        await _trainingDayRepository.Create(trainingDay);
      }

      return plan.Id;
    }

  }
}
