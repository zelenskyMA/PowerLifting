using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Создание заявки спортсменом к тренеру (по его Ид) на обучение.
    /// </summary>
    public class RequestCreateCommand : ICommand<RequestCreateCommand.Param, bool>
    {
        private readonly IProcessRequest _processTrainingRequest;
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly IUserProvider _user;

        public RequestCreateCommand(
            IProcessRequest processTrainingRequest,
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

            if (_user.Id == param.СoachId)
            {
                throw new BusinessException($"Нельзя подать заявку самому себе");
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
