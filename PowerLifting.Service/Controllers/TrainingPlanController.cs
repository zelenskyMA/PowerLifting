using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers
{

    [ApiController]
    [Route("trainingPlan")]
    public class TrainingPlanController : ControllerBase
    {
        private readonly ITrainingPlanCommands _trainingPlanCommands;
        private readonly IPlannedExerciseCommands _plannedExerciseCommands;

        public TrainingPlanController(ITrainingPlanCommands trainingPlanCommands, IPlannedExerciseCommands plannedExerciseCommands)
        {
            _trainingPlanCommands = trainingPlanCommands;
            _plannedExerciseCommands = plannedExerciseCommands;
        }

        [HttpGet]
        [Route("get")]
        public async Task<TrainingPlanModel> Get(int id)
        {
            var result = await _trainingPlanCommands.GetAsync(id);
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<int> Create([FromBody] DateTime creationDate)
        {
            var result = await _trainingPlanCommands.CreateAsync(creationDate);
            return result;
        }

        [HttpPost]
        [Route("createPlannedExercise")]
        public async Task<bool> CreatePlannedExercise(int dayId, [FromBody] List<Exercise> exercises)
        {
            await _plannedExerciseCommands.CreateAsync(dayId, exercises);
            return true;
        }
    }
}
