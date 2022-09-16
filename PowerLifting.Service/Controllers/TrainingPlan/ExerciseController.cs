using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers.TrainingPlan
{

    [Route("exercise")]
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
        [Route("getList")]
        public async Task<List<Exercise>> GetListAsync()
        {
            var result = await _exerciseCommands.GetListAsync();
            return result;
        }

        [HttpGet]
        [Route("getPercentages")]
        public async Task<List<Percentage>> GetPercentagesAsync()
        {
            var result = await _planExerciseSettingsCommands.GetPercentagesAsync();
            return result;
        }

        [HttpGet]
        [Route("getExerciseSettings")]
        public async Task<PlanExerciseSettings> GetListAsync(int id)
        {
            var result = await _planExerciseSettingsCommands.GetAsync(id);
            return result;
        }

        [HttpPost]
        [Route("updateExerciseSettings")]
        public async Task<bool> UpdateExerciseSettingsAsync([FromBody] PlanExerciseSettings settings)
        {
            await _planExerciseSettingsCommands.UpdateAsync(settings);
            return true;
        }
    }
}
