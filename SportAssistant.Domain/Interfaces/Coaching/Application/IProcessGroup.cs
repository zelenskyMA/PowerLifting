using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Domain.Interfaces.Coaching.Application;

public interface IProcessGroup
{
    /// <summary>
    /// Get user group information by user Id
    /// </summary>
    /// <param name="userId">user Id</param>
    /// <returns></returns>
    Task<TrainingGroup> GetUserGroupAsync(int userId);

    /// <summary>
    /// Получение данных о группе и пользователях, которые в ней состоят
    /// </summary>
    /// <param name="id">Ид группы</param>
    /// <returns></returns>
    Task<TrainingGroupInfo> GetGroupInfoByIdAsync(int id);

    /// <summary>
    /// Получение списка групп для текущего тренера
    /// </summary>
    /// <returns>Список групп</returns>
    Task<List<TrainingGroup>> GetGroupsListAsync();
}
