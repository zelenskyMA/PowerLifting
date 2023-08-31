using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

public interface IProcessGroupUser
{
    /// <summary>
    /// Проверяем что сущности существуют и группа принадлежит тренеру, выполняющему операцию
    /// </summary>
    Task<(TrainingGroupDb group, UserInfoDb userInfo)> CheckAssignmentAvailable(TrainingGroupUser targetGroup);

    /// <summary>
    /// Получение полного списка спортсменов у тренера
    /// </summary>
    /// <param name="coachId">Ид тренера</param>
    /// <returns></returns>
    Task<List<UserInfo>> GetCoachUsersList(int coachId);
}
