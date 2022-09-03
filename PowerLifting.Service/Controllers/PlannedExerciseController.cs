using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Service.Controllers
{
  [ApiController]
  [Route("plannedExercise")]
  public class PlannedExerciseController : ControllerBase
  {
    private readonly IPlannedExerciseApp _plannedExerciseApp;
    public PlannedExerciseController(IPlannedExerciseApp plannedExerciseApp)
    {
      _plannedExerciseApp = plannedExerciseApp;
    }

    [HttpPost]
    [Route("create")]
    public async Task Create(int trainingDayId, [FromBody] List<Exercise> exercises)
    {
      await _plannedExerciseApp.CreateAsync(trainingDayId, exercises);
    }
  }
}
