using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Создание заявки спортсменом к тренеру (по его Ид) на обучение.
    /// </summary>
    public class RequestCreateCommand : ICommand<RequestCreateCommand.Param, bool>
    {
        private readonly IProcessRequest _processTrainingRequest;
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;

        public RequestCreateCommand(
            IProcessRequest processTrainingRequest,
            ITrainingRequestRepository trainingRequestRepository,
            ICrudRepo<UserInfoDb> userInfoRepository,
            IUserProvider user)
        {
            _processTrainingRequest = processTrainingRequest;
            _trainingRequestRepository = trainingRequestRepository;
            _userInfoRepository = userInfoRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var existingRequest = await _processTrainingRequest.GetByUserAsync(_user.Id);
            var coach = await _processTrainingRequest.GetCoachName(param.СoachId);
            var userInfo = await _userInfoRepository.FindOneAsync(t => t.UserId == _user.Id);

            if (existingRequest.Id > 0)
            {
                throw new BusinessException($"Вы уже подали заявку тренеру {coach.name}");
            }

            if (coach.foundId == 0)
            {
                throw new BusinessException($"Тренер, которому вы подаете заявку, не найден.");
            }

            if (_user.Id == param.СoachId)
            {
                throw new BusinessException($"Нельзя подать заявку самому себе");
            }

            if (userInfo.CoachId != null && userInfo.CoachId > 0)
            {
                throw new BusinessException($"У вас уже есть тренер. Он может быть только один.");
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
