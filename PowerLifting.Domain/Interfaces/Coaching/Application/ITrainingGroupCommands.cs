using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Domain.Interfaces.Coaching.Application
{
    public interface ITrainingGroupCommands
    {
        /// <summary>
        /// Get current coach  training groups list
        /// </summary>
        /// <returns>Groups</returns>
        Task<List<TrainingGroup>> GetListAsync();

        /// <summary>
        /// Get group details by id
        /// </summary>
        /// <param name="id">group Id</param>
        /// <returns></returns>
        Task<TrainingGroupInfo> GetAsync(int id);

        /// <summary>
        /// Get user group information by user Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        Task<TrainingGroup> GetUserGroupAsync(int userId);

        /// <summary>
        /// Create new training group
        /// </summary>
        /// <param name="name">Group name</param>
        /// <param name="description">Group description</param>
        /// <returns></returns>
        Task CreateAsync(TrainingGroup group);

        /// <summary>
        /// Update training group
        /// </summary>
        /// <param name="group">Group to update</param>
        /// <returns></returns>
        Task UpdateAsync(TrainingGroup group);

        /// <summary>
        /// Remove training group
        /// </summary>
        /// <param name="id">Id of group to remove</param>
        /// <returns></returns>
        Task DeleteAsync(int id);
    }
}
