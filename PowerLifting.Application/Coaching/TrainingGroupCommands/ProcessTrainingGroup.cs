using AutoMapper;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingGroupCommands;

public class ProcessTrainingGroup : IProcessTrainingGroup
{
    private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
    private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
    private readonly IMapper _mapper;

    public ProcessTrainingGroup(
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
}
