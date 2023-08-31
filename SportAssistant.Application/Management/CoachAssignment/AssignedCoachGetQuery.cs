using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;

namespace SportAssistant.Application.Management.CoachAssignment;

public class AssignedCoachGetQuery : ICommand<AssignedCoachGetQuery.Param, AssignedCoach>
{
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IProcessManager _processManager;
    private readonly IProcessGroupUser _processGroupUser;
    private readonly ICrudRepo<AssignedCoachDb> _assignedCoachRepository;
    private readonly IMapper _mapper;
    private readonly IUserProvider _user;

    public AssignedCoachGetQuery(
        IProcessUserInfo processUserInfo,
        IProcessManager processManager,
        IProcessGroupUser processGroupUser,
        ICrudRepo<AssignedCoachDb> assignedCoachRepository,
        IMapper mapper,
        IUserProvider user)
    {
        _processUserInfo = processUserInfo;
        _processManager = processManager;
        _processGroupUser = processGroupUser;
        _assignedCoachRepository = assignedCoachRepository;
        _mapper = mapper;
        _user = user;
    }

    /// <inheritdoc />
    public async Task<AssignedCoach> ExecuteAsync(Param param)
    {
        param.Id = param.Id == 0 ? _user.Id : param.Id;

        var coachDb = await _assignedCoachRepository.FindOneAsync(t => t.CoachId == param.Id);
        if (coachDb == null)
        {
            throw new BusinessException("Тренер не найден");
        }

        var coach = _mapper.Map<AssignedCoach>(coachDb);

        var infoList = await _processUserInfo.GetInfoList(new List<int>() { coach.CoachId, coach.ManagerId });
        coach.CoachName = infoList.FirstOrDefault(t => t.Id == coach.CoachId)?.LegalName ?? string.Empty;
        coach.ManagerName = infoList.FirstOrDefault(t => t.Id == coach.ManagerId)?.LegalName ?? string.Empty;

        coach.Sportsmen = await _processGroupUser.GetCoachUsersList(coach.CoachId);

        var manager = await _processManager.GetBaseAsync(_user.Id);
        coach.ManagerTel = manager.TelNumber;

        return coach;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
