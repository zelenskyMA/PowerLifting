using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Удаление тренировочной группы владельцем
    /// </summary>
    public class GroupDeleteCommand : ICommand<GroupDeleteCommand.Param, bool>
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;

        public GroupDeleteCommand(
         ICrudRepo<TrainingGroupDb> trainingGroupRepository,
         ITrainingGroupUserRepository trainingGroupUserRepository)
        {
            _trainingGroupRepository = trainingGroupRepository;
            _trainingGroupUserRepository = trainingGroupUserRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var groupDb = await _trainingGroupRepository.FindAsync(t => t.Id == param.Id);
            if (!groupDb.Any())
            {
                throw new BusinessException($"Группа с Id '{param.Id}' не найдена");
            }

            var groupUsersDb = await _trainingGroupUserRepository.FindAsync(t => t.GroupId == param.Id);
            if (groupUsersDb.Any())
            {
                throw new BusinessException($"В удаляемой группе устались участники");
            }

            _trainingGroupRepository.Delete(groupDb.First());
            return true;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
