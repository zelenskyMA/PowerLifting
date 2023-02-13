using AutoMapper;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands
{
    public class ProcessRequest : IProcessRequest
    {
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public ProcessRequest(
         ITrainingRequestRepository trainingRequestRepository,
         ICrudRepo<UserInfoDb> userInfoRepository,
         IUserProvider user,
         IMapper mapper)
        {
            _trainingRequestRepository = trainingRequestRepository;
            _userInfoRepository = userInfoRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<(int foundId, string name)> GetCoachName(int userId)
        {
            var infoDb = (await _userInfoRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            var info = _mapper.Map<UserInfo>(infoDb);
            var name = UserNaming.GetLegalShortName(info?.FirstName, info?.Surname, info?.Patronimic, "Аноним");

            return (infoDb?.UserId ?? 0, name);
        }

        /// <inheritdoc />
        public async Task<TrainingRequest> GetByUserAsync(int userId)
        {
            var requestDb = (await _trainingRequestRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            if (requestDb == null)
            {
                return new TrainingRequest();
            }

            var request = _mapper.Map<TrainingRequest>(requestDb);
            request.CoachName = (await GetCoachName(request.CoachId)).name;

            return request;
        }

        /// <inheritdoc />
        public async Task RemoveAsync(int userId)
        {
            userId = userId == 0 ? _user.Id : userId;

            var requestDb = (await _trainingRequestRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            if (requestDb != null)
            {
                _trainingRequestRepository.Delete(requestDb);
            }
        }        
    }
}
