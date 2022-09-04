using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers
{
  [ApiController]
  [Route("exercise")]
  public class ExerciseController : ControllerBase
  {
    private readonly IExerciseApp _exerciseApp;
    public ExerciseController(IExerciseApp exerciseApp)
    {
      _exerciseApp = exerciseApp;
    }

    [HttpGet]
    [Route("getList")]
    public async Task<List<Exercise>> GetList()
    {
      var result = await _exerciseApp.GetListAsync();
      return result;
    }
  }
}
