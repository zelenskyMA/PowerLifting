using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.UserData.Application
{
    public interface IUserAchivementCommands
    {
        /// <summary>
        /// Get all user achivements
        /// </summary>
        /// <returns></returns>
        Task<List<UserAchivement>> GetAsync();

        /// <summary>
        /// Get user achivement by exercise type
        /// </summary>
        /// <param name="exerciseTypeId">Exercise type Id</param>
        /// <returns></returns>
        Task<UserAchivement> GetByExerciseTypeAsync(int exerciseTypeId);

        /// <summary>
        /// Create new achivement entries
        /// </summary>
        /// <param name="achivements">User achivements</param>
        Task CreateAsync(List<UserAchivement> achivements);
    }
}
