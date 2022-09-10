using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers.TrainingPlan
{

    [ApiController]
    [Route("trainingPlan")]
    public class TrainingPlanController : ControllerBase
    {
        private readonly IPlanCommands _trainingPlanCommands;
        private readonly IPlanExerciseCommands _plannedExerciseCommands;

        public TrainingPlanController(IPlanCommands trainingPlanCommands, IPlanExerciseCommands plannedExerciseCommands)
        {
            _trainingPlanCommands = trainingPlanCommands;
            _plannedExerciseCommands = plannedExerciseCommands;
        }

        [HttpGet]
        [Route("get")]
        public async Task<Plan> Get(int id)
        {
            var result = await _trainingPlanCommands.GetPlanAsync(id);
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<int> Create([FromBody] DateTime creationDate)
        {
            var result = await _trainingPlanCommands.CreateAsync(creationDate);
            return result;
        }

        [HttpGet]
        [Route("getPlanDay")]
        public async Task<PlanDay> GetPlanDay(int dayId)
        {
            var result = await _trainingPlanCommands.GetPlanDayAsync(dayId);
            return result;
        }

        [HttpGet]
        [Route("getPlanExercises")]
        public async Task<List<PlanExercise>> GetPlannedExercise(int dayId)
        {
            var result = await _plannedExerciseCommands.GetAsync(dayId);
            return result;
        }

        [HttpPost]
        [Route("createPlanExercises")]
        public async Task<bool> CreatePlannedExercise(int dayId, [FromBody] List<Exercise> exercises)
        {
            await _plannedExerciseCommands.CreateAsync(dayId, exercises);
            return true;
        }
    }
}
