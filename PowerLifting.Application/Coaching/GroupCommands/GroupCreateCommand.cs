using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Создание тренировочной группы тренером
    /// </summary>
    public class GroupCreateCommand : ICommand<GroupCreateCommand.Param, bool>
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
        private readonly IUserProvider _user;

        public GroupCreateCommand(
         ICrudRepo<TrainingGroupDb> trainingGroupRepository,
         IUserProvider user)
        {
            _trainingGroupRepository = trainingGroupRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var groupDb = await _trainingGroupRepository.FindAsync(t => t.Name == param.Group.Name && t.CoachId == _user.Id);
            if (groupDb.Any())
            {
                throw new BusinessException($"Группа с названием '{param.Group.Name}' уже существует");
            }

            await _trainingGroupRepository.CreateAsync(new TrainingGroupDb()
            {
                Name = param.Group.Name,
                Description = param.Group.Description,
                CoachId = _user.Id
            });

            return true;
        }

        public class Param
        {
            public TrainingGroup Group { get; set; }
        }
    }
}
