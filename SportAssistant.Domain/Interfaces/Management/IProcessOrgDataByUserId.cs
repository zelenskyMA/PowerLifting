using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Domain.Interfaces.Management;

public interface IProcessOrgDataByUserId
{

    /// <summary>
    /// Получение организации по ид владельца
    /// </summary>
    /// <returns></returns>
    Task<OrganizationDb?> GetOrgByUserIdAsync();

    /// <summary>
    /// Получение вычисляемой информации о компании.
    /// Например, об использованных лицензиях.
    /// </summary>
    /// <param name="orgId">Ид компании</param>
    /// <param name="maxCoaches">Кол-во лицензий в организации</param>
    /// <returns></returns>
    Task<OrgInfo> GetOrgInfoAsync(int orgId, int maxCoaches);
}
