using AutoMapper;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Получение тренеров, доступных для создания заявки спортсменом.
    /// </summary>
    public class RequestGetAvailableCoachesQuery : ICommand<RequestGetAvailableCoachesQuery.Param, List<CoachInfo>>
    {
        private readonly ITrainingRequestRepository _trainingRequestRepository;
        private readonly IMapper _mapper;

        public RequestGetAvailableCoachesQuery(
            ITrainingRequestRepository trainingRequestRepository,
            IMapper mapper)
        {
            _trainingRequestRepository = trainingRequestRepository;
            _mapper = mapper;
        }

        public async Task<List<CoachInfo>> ExecuteAsync(Param param)
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

        public class Param { }
    }
}
