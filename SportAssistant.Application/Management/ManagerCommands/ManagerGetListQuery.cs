using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Management;

namespace SportAssistant.Application.Management.ManagerCommands;

public class ManagerGetListQuery : ICommand<ManagerGetListQuery.Param, List<Manager>>
{
    private readonly IProcessManager _processManager;

    public ManagerGetListQuery(IProcessManager processManager)
    {
        _processManager = processManager;
    }

    /// <inheritdoc />
    public async Task<List<Manager>> ExecuteAsync(Param param)
    {
        var result = await _processManager.GetListAsync(param.OrgId);
        return result;
    }

    public class Param
    {
        public int OrgId { get; set; }
    }
}
