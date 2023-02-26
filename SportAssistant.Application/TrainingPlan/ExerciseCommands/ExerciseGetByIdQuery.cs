using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.ExerciseCommands
{
    /// <summary>
    /// Get exercise by it's Id
    /// </summary>
    public class ExerciseGetByIdQuery : ICommand<ExerciseGetByIdQuery.Param, Exercise>
    {
        private readonly IProcessExercise _processExercise;

        public ExerciseGetByIdQuery(IProcessExercise processExercise)
        {
            _processExercise = processExercise;
        }

        public async Task<Exercise> ExecuteAsync(Param param)
        {
            var result = await _processExercise.GetAsync(param.Id) ?? new Exercise();
            return result;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
