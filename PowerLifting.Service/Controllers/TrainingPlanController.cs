using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Service.Controllers
{

  [ApiController]
  [Route("trainingPlan")]
  public class TrainingPlanController : ControllerBase
  {
    private readonly ITrainingPlanApp _trainingPlanApp;

    public TrainingPlanController(ITrainingPlanApp trainingPlanApp)
    {
      _trainingPlanApp = trainingPlanApp;
    }

    [HttpGet]
    public async Task<TrainingPlan> Get(int id)
    {
      return await _trainingPlanApp.GetAsync(id);
    }

    [HttpPost]
    [Route("create")]
    public async Task<int> Create(DateTime creationDate)
    {
      var result = await _trainingPlanApp.CreateAsync(creationDate);
      return result;
    }
  }
}
