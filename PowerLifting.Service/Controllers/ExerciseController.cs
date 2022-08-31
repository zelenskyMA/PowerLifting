using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Models;

namespace PowerLifting.Service.Controllers
{
  [ApiController]
  [Route("trainingPlan")]
  public class ExerciseController : ControllerBase
  {
    private readonly IExerciseApp _exerciseApp;

    public ExerciseController(IExerciseApp exerciseApp)
    {
      _exerciseApp = exerciseApp;
    }

    [HttpGet]
    [Route("getTypes")]
    public async Task<List<DictionaryItem>> GetExerciseTypes()
    {
      var result = await _exerciseApp.GetTypesAsync();
      return result;
    }
  }
}
