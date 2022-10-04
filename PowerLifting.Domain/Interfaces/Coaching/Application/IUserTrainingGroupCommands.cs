using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Domain.Interfaces.Coaching.Application
{
    public interface IUserTrainingGroupCommands
    {
        /// <summary>
        /// Add or Change user training group. Set user coach.
        /// </summary>
        /// <param name="targetGroup">Target group and user Id</param>
        /// <returns></returns>
        Task UpdateUserGroup(UserTrainingGroup targetGroup);

        /// <summary>
        /// Remove user from training group and coach.
        /// </summary>
        /// <param name="targetGroup">Target group and user Id</param>
        /// <returns></returns>
        Task RemoveUserFromGroup(UserTrainingGroup targetGroup);

        /// <summary>
        /// User rejects coach
        /// </summary>
        /// <returns></returns>
        Task RejectCoach();
    }
}
