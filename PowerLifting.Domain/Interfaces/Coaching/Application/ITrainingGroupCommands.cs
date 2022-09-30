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
