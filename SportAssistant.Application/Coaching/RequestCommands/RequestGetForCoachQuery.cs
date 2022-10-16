using AutoMapper;
using SportAssistant.Application.Common;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Получение тренером списка заявок, поданных ему пользователями
    /// </summary>
    public class RequestGetForCoachQuery : ICommand<RequestGetForCoachQuery.Param, List<TrainingRequest>>
    {
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public RequestGetForCoachQuery(
            ITrainingRequestRepository trainingRequestRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _trainingRequestRepository = trainingRequestRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<List<TrainingRequest>> ExecuteAsync(Param param)
        {
            var requestsDb = await _trainingRequestRepository.FindAsync(t => t.CoachId == _user.Id);
            if (requestsDb.Count == 0)
            {
                return new List<TrainingRequest>();
            }

            var usersInfoDb = await _trainingRequestRepository.GetUsersAsync(requestsDb.Select(t => t.UserId).ToList());

            var requests = requestsDb.Select(t => _mapper.Map<TrainingRequest>(t)).OrderByDescending(t => t.CreationDate).ToList();
            foreach (var item in requests)
            {
                var userInfoDb = usersInfoDb.First(t => t.UserId == item.UserId);
                item.UserName = Naming.GetLegalFullName(userInfoDb.FirstName, userInfoDb.Surname, userInfoDb.Patronimic);
            }

            return requests;
        }

        public class Param { }
    }
}
