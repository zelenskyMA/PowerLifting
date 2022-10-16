using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessGroupUser
    {
        /// <summary>
        /// Проверяем что сущности существуют и группа принадлежит тренеру, выполняющему операцию
        /// </summary>
        Task<(TrainingGroupDb group, UserInfoDb userInfo)> CheckAssignmentAvailable(TrainingGroupUser targetGroup);
    }
}
