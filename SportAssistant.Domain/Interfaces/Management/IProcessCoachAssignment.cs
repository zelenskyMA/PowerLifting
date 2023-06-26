using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Domain.Interfaces.Management;

public interface IProcessCoachAssignment
{
    /// <summary>
    /// Удаление тренерских лицензий при удалении менеджера
    /// </summary>
    /// <param name="managerId">Ид удаляемого мэнеджера</param>
    /// <returns></returns>
    Task<bool> DropCoachesByManagerAsync(int managerId);

    /// <summary>
    /// Переподчинение тренеров
    /// </summary>
    /// <param name="managerId">Ид текущего менеджера</param>
    /// <param name="newManagerId">Ид нового менеджера</param>
    /// <returns></returns>
    Task<bool> ReassignCoachesAsync(int managerId, int newManagerId);

    /// <summary>
    /// Получение списка ид подчиненных менеджерам трениров
    /// </summary>
    /// <param name="managerIds">Список ид менеджеров</param>
    /// <returns></returns>
    Task<List<ManagerCoaches>> GetAssignedCoachesAsync(List<int> managerIds);
}
