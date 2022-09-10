using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Domain.Interfaces.UserData.Application
{
    public interface IUserAchivementCommands
    {
        /// <summary>
        /// Get user achivement by exercise type
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="exerciseTypeId">Exercise type Id</param>
        /// <returns></returns>
        Task<UserAchivement> GetByExerciseType(int userId, int exerciseTypeId);
    }
}
