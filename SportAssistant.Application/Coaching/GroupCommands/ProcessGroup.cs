using AutoMapper;
using SportAssistant.Application.Common.Actions;
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
    private readonly IMapper _mapper;

    public ProcessGroup(
     ICrudRepo<TrainingGroupDb> trainingGroupRepository,
     ITrainingGroupUserRepository trainingGroupUserRepository,
     IMapper mapper)
    {
        _trainingGroupRepository = trainingGroupRepository;
        _trainingGroupUserRepository = trainingGroupUserRepository;
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

        return groupInfo;
    }
}
