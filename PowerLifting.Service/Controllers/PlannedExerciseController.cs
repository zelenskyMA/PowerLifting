using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers
{
    [ApiController]
    [Route("plannedExercise")]
    public class PlannedExerciseController : ControllerBase
    {
        private readonly IPlannedExerciseCommands _plannedExerciseApp;
        public PlannedExerciseController(IPlannedExerciseCommands plannedExerciseApp)
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
