using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Обновление тренировочной группы тренером
    /// </summary>
    public class GroupUpdateCommand : ICommand<GroupUpdateCommand.Param, bool>
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;

        public GroupUpdateCommand(
         ICrudRepo<TrainingGroupDb> trainingGroupRepository)
        {
            _trainingGroupRepository = trainingGroupRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == param.Group.Id);
            if (!groupsDb.Any())
            {
                throw new BusinessException($"Группа не существует");
            }

            var groupDb = groupsDb.First();
            if (groupDb.Name != param.Group.Name)
            {
                var duplicateGroupsDb = await _trainingGroupRepository.FindAsync(t => t.Name == param.Group.Name);
                if (groupsDb.Count() > 0)
                {
                    throw new BusinessException($"Группа с названием '{param.Group.Name}' уже существует. Измените название на другое.");
                }
            }

            groupDb.Description = param.Group.Description;
            groupDb.Name = param.Group.Name;

            _trainingGroupRepository.Update(groupDb);
            return true;
        }

        public class Param
        {
            public TrainingGroup Group { get; set; }
        }
    }
}
