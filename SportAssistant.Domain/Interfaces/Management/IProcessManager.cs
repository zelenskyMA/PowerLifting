using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Domain.Interfaces.Management;

public interface IProcessManager
{
    /// <summary>
    /// Получение базовой информации по менеджеру.
    /// </summary>
    /// <param name="managerId">Ид менеджера</param>
    /// <returns></returns>
    Task<Manager> GetBaseAsync(int managerId);

    /// <summary>
    /// Получение списка менеджеров организации по ее Ид.
    /// </summary>
    /// <param name="orgId">Ид организации</param>
    /// <returns></returns>
    Task<List<Manager>> GetListAsync(int orgId);
}
