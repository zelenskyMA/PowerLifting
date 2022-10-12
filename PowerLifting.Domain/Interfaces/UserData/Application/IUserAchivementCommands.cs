using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.UserData.Application
{
    public interface IProcessUserAchivements
    {
        /// <summary>
        /// Get all user achivements
        /// </summary>
        /// <returns></returns>
        Task<List<UserAchivement>> GetAsync(int userId);

        /// <summary>
        /// Get user achivement by exercise type
        /// </summary>
        /// <param name="exerciseTypeId">Exercise type Id</param>
        /// <returns></returns>
        Task<UserAchivement> GetByExerciseTypeAsync(int userId, int exerciseTypeId);
    }
}
