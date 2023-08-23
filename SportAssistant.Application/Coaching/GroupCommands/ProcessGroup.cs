using AutoMapper;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupCommands;

public class ProcessGroup : IProcessGroup
{
    private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
    private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public ProcessGroup(
     ICrudRepo<TrainingGroupDb> trainingGroupRepository,
     ITrainingGroupUserRepository trainingGroupUserRepository,
     IUserProvider user,
     IMapper mapper)
    {
        _trainingGroupRepository = trainingGroupRepository;
        _trainingGroupUserRepository = trainingGroupUserRepository;
        _user = user;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TrainingGroup> GetUserGroupAsync(int userId)
    {
        var userGroupDb = (await _trainingGroupUserRepository.FindAsync(t => t.UserId == userId))?.FirstOrDefault();
        if (userGroupDb == null)
        {
            return new TrainingGroup();
        }

        var groupDb = (await _trainingGroupRepository.FindAsync(t => t.Id == userGroupDb.GroupId)).FirstOrDefault();
        if (groupDb == null)
        {
            throw new BusinessException("Группа не найдена");
        }

        return _mapper.Map<TrainingGroup>(groupDb);
    }

    /// <inheritdoc />
    public async Task<TrainingGroupInfo> GetGroupInfoByIdAsync(int id)
    {
        var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == id);
        if (!groupsDb.Any())
        {
            throw new BusinessException("Группа не найдена");
        }

        var usersInfoDb = await _trainingGroupUserRepository.GetGroupUsersAsync(id);
        var userIds = usersInfoDb.Select(t => t.UserId).ToList();

        var users = usersInfoDb.Select(t => new GroupUser()
        {
            Id = t.UserId,
            FullName = UserNaming.GetLegalFullName(t.FirstName, t.Surname, t.Patronimic)
        }).OrderBy(t => t.FullName).ToList();

        var groupInfo = new TrainingGroupInfo()
        {
            Group = _mapper.Map<TrainingGroup>(groupsDb.First()),
            Users = users
        };

        groupInfo.Group.ParticipantsCount = users.Count();

        return groupInfo;
    }

    /// <inheritdoc />
    public async Task<List<TrainingGroup>> GetGroupsListAsync()
    {
        var groupsDb = await _trainingGroupRepository.FindAsync(t => t.CoachId == _user.Id);
        if (!groupsDb.Any())
        {
            return new List<TrainingGroup>();
        }

        var groups = groupsDb.Select(_mapper.Map<TrainingGroup>).OrderBy(t => t.Name).ToList();
        foreach (var item in groups)
        {
            item.ParticipantsCount = (await _trainingGroupUserRepository.FindAsync(t => t.GroupId == item.Id)).Count();
        }

        return groups;
    }
}
