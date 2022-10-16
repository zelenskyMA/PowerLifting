using AutoMapper;
using PowerLifting.Application.Common;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Получение тренером поданной к нему заявки по ее Ид.
    /// </summary>
    public class TrainingRequestGetForCoachSingleQuery : ICommand<TrainingRequestGetForCoachSingleQuery.Param, TrainingRequest>
    {
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TrainingRequestGetForCoachSingleQuery(
            ITrainingRequestRepository trainingRequestRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _trainingRequestRepository = trainingRequestRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<TrainingRequest> ExecuteAsync(Param param)
        {
            var requestsDb = await _trainingRequestRepository.FindAsync(t => t.CoachId == _user.Id && t.Id == param.Id);
            if (requestsDb.Count == 0)
            {
                throw new BusinessException($"У тренера с Ид {_user.Id} нет заявки с Ид {param.Id}");
            }

            var usersInfoDb = await _trainingRequestRepository.GetUsersAsync(requestsDb.Select(t => t.UserId).ToList());

            var request = requestsDb.Select(t => _mapper.Map<TrainingRequest>(t)).First();
            var userInfoDb = usersInfoDb.First(t => t.UserId == request.UserId);
            request.UserName = Naming.GetLegalFullName(userInfoDb.FirstName, userInfoDb.Surname, userInfoDb.Patronimic);
            request.UserWeight = userInfoDb.Weight ?? 0;
            request.UserHeight = userInfoDb.Height ?? 0;
            request.UserAge = userInfoDb.Age ?? 0;

            return request;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
