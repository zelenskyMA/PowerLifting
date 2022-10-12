using AutoMapper;
using PowerLifting.Application.Common;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching;

public class TrainingGroupCommands : ITrainingGroupCommands
{
    private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
    private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
    private readonly IUserTrainingGroupRepository _userTrainingGroupRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public TrainingGroupCommands(
     ICrudRepo<TrainingGroupDb> trainingGroupRepository,
     ICrudRepo<PlanDb> trainingPlanRepository,
     IUserTrainingGroupRepository userTrainingGroupRepository,
     IUserProvider user,
     IMapper mapper)
    {
        _trainingGroupRepository = trainingGroupRepository;
        _trainingPlanRepository = trainingPlanRepository;
        _userTrainingGroupRepository = userTrainingGroupRepository;
        _user = user;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<List<TrainingGroup>> GetListAsync()
    {
        var groupsDb = await _trainingGroupRepository.FindAsync(t => t.CoachId == _user.Id);
        if (!groupsDb.Any())
        {
            return new List<TrainingGroup>();
        }

        var groups = groupsDb.Select(t => _mapper.Map<TrainingGroup>(t)).OrderBy(t => t.Name).ToList();
        foreach (var item in groups)
        {
            item.ParticipantsCount = (await _userTrainingGroupRepository.FindAsync(t => t.GroupId == item.Id)).Count();
        }

        return groups.OrderBy(t => t.Name).ToList();
    }

    /// <inheritdoc />
    public async Task<TrainingGroupInfo> GetAsync(int id)
    {
        var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == id);
        if (!groupsDb.Any())
        {
            throw new BusinessException("Группа не найдена");
        }

        var usersInfoDb = await _userTrainingGroupRepository.GetGroupUsersAsync(id);
        var userIds = usersInfoDb.Select(t => t.UserId).ToList();
        var allActivePlans = await _trainingPlanRepository.FindAsync(
            t => userIds.Contains(t.UserId) && t.StartDate.AddDays(7) >= DateTime.Now.Date);

        var users = usersInfoDb.Select(t => new GroupUser()
        {
            Id = t.UserId,
            FullName = Naming.GetLegalFullName(t.FirstName, t.Surname, t.Patronimic),
            ActivePlansCount = allActivePlans.Count(t => t.UserId == t.UserId),
        }).OrderBy(t => t.FullName).ToList();

        var groupInfo = new TrainingGroupInfo()
        {
            Group = _mapper.Map<TrainingGroup>(groupsDb.First()),
            Users = users
        };

        return groupInfo;
    }

    /// <inheritdoc />
    public async Task<TrainingGroup> GetUserGroupAsync(int userId)
    {
        var userGroupDb = (await _userTrainingGroupRepository.FindAsync(t => t.UserId == userId))?.FirstOrDefault();
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
    public async Task CreateAsync(TrainingGroup group)
    {
        var groupDb = await _trainingGroupRepository.FindAsync(t => t.Name == group.Name && t.CoachId == _user.Id);
        if (groupDb.Any())
        {
            throw new BusinessException($"Группа с названием '{group.Name}' уже существует");
        }

        await _trainingGroupRepository.CreateAsync(new TrainingGroupDb()
        {
            Name = group.Name,
            Description = group.Description,
            CoachId = _user.Id
        });
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TrainingGroup group)
    {
        var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == group.Id);
        if (!groupsDb.Any())
        {
            throw new BusinessException($"Группа не существует");
        }

        var groupDb = groupsDb.First();
        if (groupDb.Name != group.Name)
        {
            var duplicateGroupsDb = await _trainingGroupRepository.FindAsync(t => t.Name == group.Name);
            if (groupsDb.Count() > 0)
            {
                throw new BusinessException($"Группа с названием '{group.Name}' уже существует. Измените название на другое.");
            }
        }

        groupDb.Description = group.Description;
        groupDb.Name = group.Name;

        _trainingGroupRepository.Update(groupDb);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        var groupDb = await _trainingGroupRepository.FindAsync(t => t.Id == id);
        if (!groupDb.Any())
        {
            throw new BusinessException($"Группа с Id '{id}' не найдена");
        }

        var groupUsersDb = await _userTrainingGroupRepository.FindAsync(t => t.GroupId == id);
        if (groupUsersDb.Any())
        {
            throw new BusinessException($"В удаляемой группе устались участники");
        }

        _trainingGroupRepository.Delete(groupDb.First());
    }
}
