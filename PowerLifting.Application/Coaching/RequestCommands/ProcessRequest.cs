using AutoMapper;
using PowerLifting.Application.Common;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
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
        public async Task<string> GetCoachName(int userId)
        {
            var infoDb = (await _userInfoRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            var info = _mapper.Map<UserInfo>(infoDb);
            return Naming.GetLegalShortName(info.FirstName, info.Surname, info.Patronimic, "Аноним");
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
            request.CoachName = await GetCoachName(request.CoachId);

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
