using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.Management;

namespace SportAssistant.Application.Management.CoachAssignment;

public class ProcessCoachAssignment : IProcessCoachAssignment
{
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly ICrudRepo<AssignedCoachDb> _assignedCoachRepository;

    public ProcessCoachAssignment(
        IUserRoleCommands userRoleCommands,
        ICrudRepo<AssignedCoachDb> assignedCoachRepository)
    {
        _userRoleCommands = userRoleCommands;
        _assignedCoachRepository = assignedCoachRepository;
    }

    /// <inheritdoc />
    public async Task<bool> DropCoachesByManagerAsync(int managerId)
    {
        var assignedCoaches = await _assignedCoachRepository.FindAsync(t => t.ManagerId == managerId);
        if (assignedCoaches.Count == 0)
        {
            return true;
        }

        foreach (var item in assignedCoaches)
        {
            await _userRoleCommands.RemoveRole(item.CoachId, UserRoles.Coach);
        }

        _assignedCoachRepository.DeleteList(assignedCoaches);

        return true;
    }

    /// <inheritdoc />
    public async Task<int> ReassignCoachesAsync(int managerId, int newManagerId, List<int>? coachIds = null)
    {
        var assignedCoaches = await _assignedCoachRepository.FindAsync(t => t.ManagerId == managerId);
        if (coachIds != null && coachIds.Count > 0)
        {
            assignedCoaches = assignedCoaches.Where(t => coachIds.Contains(t.CoachId)).ToList();
        }

        if (assignedCoaches.Count == 0)
        {
            return 0;
        }

        _assignedCoachRepository.DeleteList(assignedCoaches);

        var newList = assignedCoaches.Select(t=> new AssignedCoachDb() { CoachId = t.CoachId, ManagerId = newManagerId }).ToList();
        await _assignedCoachRepository.CreateListAsync(newList);

        return assignedCoaches.Count;
    }

    /// <inheritdoc />
    public async Task<List<ManagerCoaches>> GetAssignedCoachesAsync(List<int> managerIds)
    {
        var coaches = await _assignedCoachRepository.FindAsync(t => managerIds.Contains(t.ManagerId));

        var managers = new List<ManagerCoaches>();
        foreach (var id in managerIds)
        {
            managers.Add(new ManagerCoaches()
            {
                ManagerId = id,
                CoachIds = coaches.Where(c => c.ManagerId == id).Select(t => t.CoachId).ToList(),
            });
        };

        return managers;
    }
}
