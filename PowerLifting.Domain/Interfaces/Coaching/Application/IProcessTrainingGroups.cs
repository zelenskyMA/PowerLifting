using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Domain.Interfaces.Coaching.Application
{
    public interface IProcessTrainingGroup
    {
        /// <summary>
        /// Get user group information by user Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        Task<TrainingGroup> GetUserGroupAsync(int userId);
    }
}
