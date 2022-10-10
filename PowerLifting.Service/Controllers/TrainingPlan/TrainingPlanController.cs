using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.TrainingPlan;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers.TrainingPlan
{

    [Route("trainingPlan")]
    public class TrainingPlanController : BaseController
    {
        private readonly IPlanCommands _trainingPlanCommands;
        private readonly IPlanExerciseCommands _plannedExerciseCommands;

        public TrainingPlanController(IPlanCommands trainingPlanCommands, IPlanExerciseCommands plannedExerciseCommands)
        {
            _trainingPlanCommands = trainingPlanCommands;
            _plannedExerciseCommands = plannedExerciseCommands;
        }

        [HttpGet]
        [Route("getList")]
        public async Task<Plans> GetListAsync(int userId = 0)
        {
            var result = await _trainingPlanCommands.GetPlansAsync(userId);
            return result;
        }

        [HttpGet]
        [Route("get")]
        public async Task<Plan> GetAsync(int id)
        {
            var result = await _trainingPlanCommands.GetPlanAsync(id);
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<int> CreateAsync(RequestPlanCreation request)
        {
            var result = await _trainingPlanCommands.CreateAsync(request);
            return result;
        }

        [HttpGet]
        [Route("getPlanDay")]
        public async Task<PlanDay> GetPlanDayAsync(int dayId)
        {
            var result = await _trainingPlanCommands.GetPlanDayAsync(dayId);
            return result;
        }

        [HttpGet]
        [Route("getCurrentDay")]
        public async Task<PlanDay> GetCurrentDayAsync()
        {
            var result = await _trainingPlanCommands.GetCurrentDayAsync();
            return result;
        }

        [HttpGet]
        [Route("getPlanExercises")]
        public async Task<List<PlanExercise>> GetPlannedExercisesAsync(int dayId)
        {
            var result = await _plannedExerciseCommands.GetAsync(dayId);
            return result;
        }

        [HttpPost]
        [Route("createPlanExercises")]
        public async Task<bool> CreatePlannedExerciseAsync(int dayId, [FromBody] List<Exercise> exercises)
        {
            await _plannedExerciseCommands.CreateAsync(dayId, exercises);
            return true;
        }

        [HttpGet]
        [Route("getPlanExercise")]
        public async Task<PlanExercise> GetPlannedExerciseAsync(int id)
        {
            var result = await _plannedExerciseCommands.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [Route("updatePlanExercise/{userId:int}")]
        public async Task<bool> UpdatePlanExerciseAsync(int userId, PlanExercise planExercise)
        {
            await _plannedExerciseCommands.UpdateAsync(userId, planExercise);
            return true;
        }
    }
}
