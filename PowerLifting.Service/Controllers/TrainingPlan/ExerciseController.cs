using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers.TrainingPlan
{

    [Route("exerciseInfo")]
    public class ExerciseController : BaseController
    {
        private readonly IExerciseCommands _exerciseCommands;
        private readonly IPlanExerciseSettingsCommands _planExerciseSettingsCommands;

        public ExerciseController(IExerciseCommands exerciseCommands, IPlanExerciseSettingsCommands planExerciseSettingsCommands)
        {
            _exerciseCommands = exerciseCommands;
            _planExerciseSettingsCommands = planExerciseSettingsCommands;
        }

        [HttpGet]
        [Route("get")]
        public async Task<Exercise> Get(int id)
        {
            var result = await _exerciseCommands.GetAsync(id);
            return result ?? new Exercise();
        }

        [HttpGet]
        [Route("getPlanPercentages")]
        public async Task<List<Percentage>> GetPlanPercentagesAsync(int dayId)
        {
            var result = await _planExerciseSettingsCommands.GetPercentageListAsync();
            return result;
        }

        [HttpGet]
        [Route("getExerciseSettings")]
        public async Task<PlanExerciseSettings> GetExerciseSettingsAsync(int id)
        {
            var result = await _planExerciseSettingsCommands.GetAsync(id);
            return result;
        }
               

        [HttpGet]
        [Route("getPlanningList")]
        public async Task<List<Exercise>> GetListAsync()
        {
            var result = await _exerciseCommands.GetPlanningListAsync();
            return result;
        }

        [HttpGet]
        [Route("getEditingList")]
        public async Task<List<Exercise>> GetEditingListAsync()
        {
            var result = await _exerciseCommands.GetEditingListAsync();
            return result;
        }

        [HttpGet]
        [Route("getAdminEditingList")]
        public async Task<List<Exercise>> GetAdminEditingListAsync()
        {
            var result = await _exerciseCommands.GetAdminEditingListAsync();
            return result;
        }

        [HttpPost]
        [Route("updateExercise")]
        public async Task<bool> UpdateExerciseAsync(Exercise exercise)
        {
            await _exerciseCommands.UpdateAsync(exercise);
            return true;
        }

        [HttpPost]
        [Route("deleteExercise")]
        public async Task<bool> DeleteExerciseAsync(int id)
        {
            await _exerciseCommands.DeleteAsync(id);
            return true;
        }
    }
}
