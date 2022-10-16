using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.ExerciseCommands
{
    /// <summary>
    /// Get exercises by userId list
    /// </summary>
    public class ExerciseGetByUsersQuery : ICommand<ExerciseGetByUsersQuery.Param, List<Exercise>>
    {
        private readonly IProcessExercise _processExercise;

        public ExerciseGetByUsersQuery(IProcessExercise processExercise)
        {
            _processExercise = processExercise;
        }

        public async Task<List<Exercise>> ExecuteAsync(Param param)
        {
            var result = await _processExercise.GetAllowedExercises(param.UserIds);
            return result;
        }

        public class Param
        {
            public int?[] UserIds { get; set; } = new int?[] { };
        }
    }
}
