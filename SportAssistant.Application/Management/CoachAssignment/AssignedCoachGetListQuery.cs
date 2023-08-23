using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.CoachAssignment;

public class AssignedCoachGetListQuery : ICommand<AssignedCoachGetListQuery.Param, List<AssignedCoach>>
{
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<AssignedCoachDb> _assignedCoachRepository;
    private readonly IMapper _mapper;
    private readonly IUserProvider _user;

    public AssignedCoachGetListQuery(
        IProcessUserInfo processUserInfo,
        ICrudRepo<AssignedCoachDb> assignedCoachRepository,
        IMapper mapper,
        IUserProvider user)
    {
        _processUserInfo = processUserInfo;
        _assignedCoachRepository = assignedCoachRepository;
        _mapper = mapper;
        _user = user;
    }

    /// <inheritdoc />
    public async Task<List<AssignedCoach>> ExecuteAsync(Param param)
    {
        var coachListDb = await _assignedCoachRepository.FindAsync(t=> t.ManagerId == _user.Id);
        var coachList = coachListDb.Select(_mapper.Map<AssignedCoach>).ToList();

        var infoList = await _processUserInfo.GetInfoList(coachList.Select(t => t.CoachId).ToList());
        foreach (var coach in coachList)
        {
            coach.CoachName = infoList.FirstOrDefault(t => t.Id == coach.CoachId)?.LegalName ?? string.Empty;
        }

        return coachList;
    }

    public class Param
    {
    }
}
