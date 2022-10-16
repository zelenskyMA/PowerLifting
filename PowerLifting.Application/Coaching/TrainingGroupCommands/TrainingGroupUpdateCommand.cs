using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Обновление тренировочной группы тренером
    /// </summary>
    public class TrainingGroupUpdateCommand : ICommand<TrainingGroupUpdateCommand.Param, bool>
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;

        public TrainingGroupUpdateCommand(
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
