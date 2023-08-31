using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Domain.Interfaces.Coaching.Repositories;

public interface ITrainingGroupUserRepository : ICrudRepo<TrainingGroupUserDb>
{
    /// <summary>
    /// Получить список пользователей в выбранной группе
    /// </summary>
    /// <param name="groupId">Ид группы</param>
    /// <returns></returns>
    Task<List<UserInfoDb>> GetGroupUsersAsync(int groupId);

    /// <summary>
    /// Получить список всех спортсменов тренера (во всех группах)
    /// </summary>
    /// <param name="coachId">Ид тренера</param>
    /// <returns></returns>
    Task<List<UserInfoDb>> GetCoachUsersFullListAsync(int coachId);
}
