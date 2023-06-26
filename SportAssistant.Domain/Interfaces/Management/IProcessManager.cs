using SportAssistant.Domain.DbModels.Management;

namespace SportAssistant.Domain.Interfaces.Management;

public interface IProcessManager
{
    /// <summary>
    /// Получение списка менеджеров организации по ее Ид.
    /// </summary>
    /// <param name="orgId">Ид организации</param>
    /// <returns></returns>
    Task<List<Manager>> GetListAsync(int orgId);
}
