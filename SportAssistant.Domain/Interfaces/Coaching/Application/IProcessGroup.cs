using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Domain.Interfaces.Coaching.Application
{
    public interface IProcessGroup
    {
        /// <summary>
        /// Get user group information by user Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        Task<TrainingGroup> GetUserGroupAsync(int userId);
    }
}
