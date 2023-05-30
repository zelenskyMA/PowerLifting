using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

public interface IProcessGroupUser
{
    /// <summary>
    /// Проверяем что сущности существуют и группа принадлежит тренеру, выполняющему операцию
    /// </summary>
    Task<(TrainingGroupDb group, UserInfoDb userInfo)> CheckAssignmentAvailable(TrainingGroupUser targetGroup);
}
