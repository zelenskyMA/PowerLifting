using AutoMapper;
using PowerLifting.Application.Common;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.Coaching
{
    public class TrainingRequestCommands : ITrainingRequestCommands
    {
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TrainingRequestCommands(
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
        public async Task<TrainingRequest> GetMyRequestAsync()
        {
            var requestDb = (await _trainingRequestRepository.FindAsync(t => t.UserId == _user.Id)).FirstOrDefault();
            if (requestDb == null)
            {
                return new TrainingRequest();
            }

            var request = _mapper.Map<TrainingRequest>(requestDb);
            request.CoachName = await GetCoachName(request.CoachId);

            return request;
        }

        /// <inheritdoc />
        public async Task<List<TrainingRequest>> GetCoachRequestsAsync()
        {
            var requestsDb = await _trainingRequestRepository.FindAsync(t => t.CoachId == _user.Id);
            if (requestsDb.Count == 0)
            {
                return new List<TrainingRequest>();
            }

            return requestsDb.Select(t => _mapper.Map<TrainingRequest>(t)).ToList();
        }

        /// <inheritdoc />
        public async Task<List<CoachInfo>> GetCoachesAsync()
        {
            var coachesDb = await _trainingRequestRepository.GetCoachesAsync();
            if (coachesDb.Count == 0)
            {
                return new List<CoachInfo>();
            }

            var coaches = coachesDb.Select(t => _mapper.Map<CoachInfo>(t)).ToList();
            foreach (var item in coaches)
            {
                var coachDb = coachesDb.First(t => t.UserId == item.Id);
                item.Name = string.Join(" ", new[] { coachDb.Surname, coachDb.FirstName, coachDb.Patronimic });
            }

            return coaches;
        }

        /// <inheritdoc />
        public async Task CreateAsync(int coachId)
        {
            var existingRequest = await GetMyRequestAsync();
            if (existingRequest.Id > 0)
            {
                var coachName = await GetCoachName(coachId);
                throw new BusinessException($"Вы уже подали заявку тренеру {coachName}");
            }

            await _trainingRequestRepository.CreateAsync(new TrainingRequestDb()
            {
                UserId = _user.Id,
                CoachId = coachId,
                CreationDate = DateTime.UtcNow
            });
        }

        /// <inheritdoc />
        public async Task RemoveRequestAsync(int userId)
        {
            userId = userId == 0 ? _user.Id : userId;

            var requestDb = (await _trainingRequestRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            if (requestDb != null)
            {
                await _trainingRequestRepository.DeleteAsync(requestDb);
            }
        }

        private async Task<string> GetCoachName(int userId)
        {
            var infoDb = (await _userInfoRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            var info = _mapper.Map<UserInfo>(infoDb);
            return Naming.GetLegalName(info.FirstName, info.Surname, info.Patronimic, "Аноним");
        }
    }
}
