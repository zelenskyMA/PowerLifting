using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IExerciseCommands
    {
        Task<List<Exercise>> GetListAsync();
    }
}
