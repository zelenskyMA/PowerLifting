using AutoMapper;
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
}
