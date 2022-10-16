using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Actions;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Создание заявки спортсменом к тренеру (по его Ид) на обучение.
    /// </summary>
    public class TrainingRequestCreateCommand : ICommand<TrainingRequestCreateCommand.Param, bool>
    {
        private readonly IProcessTrainingRequest _processTrainingRequest;
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly IUserProvider _user;

        public TrainingRequestCreateCommand(
            IProcessTrainingRequest processTrainingRequest,
            ITrainingRequestRepository trainingRequestRepository,
            IUserProvider user)
        {
            _processTrainingRequest = processTrainingRequest;
            _trainingRequestRepository = trainingRequestRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var existingRequest = await _processTrainingRequest.GetByUserAsync(_user.Id);
            if (existingRequest.Id > 0)
            {
                var coachName = await _processTrainingRequest.GetCoachName(param.СoachId);
                throw new BusinessException($"Вы уже подали заявку тренеру {coachName}");
            }

            await _trainingRequestRepository.CreateAsync(new TrainingRequestDb()
            {
                UserId = _user.Id,
                CoachId = param.СoachId,
                CreationDate = DateTime.UtcNow
            });

            return true;
        }

        public class Param
        {
            public int СoachId { get; set; }
        }
    }
}
