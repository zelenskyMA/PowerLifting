using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.ExerciseCommands
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
