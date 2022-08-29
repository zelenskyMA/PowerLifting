using AutoMapper;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Domain.Models.TrainingWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLifting.Application
{
  public class TrainingPlanApp : ITrainingPlanApp
  {
    private readonly ITrainingPlanRepository _trainingPlanRepository;
    private readonly IMapper _mapper;

    public TrainingPlanApp(ITrainingPlanRepository trainingPlanRepository, IMapper mapper)
    {
      _trainingPlanRepository = trainingPlanRepository;
      _mapper = mapper;
    }

    public async Task<TrainingPlan> GetAsync(int Id)
    {
      var dbPlan = (await _trainingPlanRepository.FindAsync(t => t.Id == Id)).FirstOrDefault();
      if (dbPlan == null)
      {
        return null;
      }

      var plan = _mapper.Map<TrainingPlan>(dbPlan);

      return plan;
    }

    public async Task<int> UpdateAsync(TrainingPlan plan)
    {
      var dbPlan = _mapper.Map<TrainingPlanDb>(plan);

      await _trainingPlanRepository.Create(dbPlan);

      return dbPlan.Id;
    }

  }
}
