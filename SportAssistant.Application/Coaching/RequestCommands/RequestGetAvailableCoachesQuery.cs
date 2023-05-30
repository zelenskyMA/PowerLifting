using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands;

/// <summary>
/// Получение тренеров, доступных для создания заявки спортсменом.
/// </summary>
public class RequestGetAvailableCoachesQuery : ICommand<RequestGetAvailableCoachesQuery.Param, List<CoachInfo>>
{
    private readonly ITrainingRequestRepository _trainingRequestRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public RequestGetAvailableCoachesQuery(
        ITrainingRequestRepository trainingRequestRepository,
        IUserProvider user,
        IMapper mapper)
    {
        _trainingRequestRepository = trainingRequestRepository;
        _user = user;
        _mapper = mapper;
    }

    public async Task<List<CoachInfo>> ExecuteAsync(Param param)
    {
        var coachesDb = await _trainingRequestRepository.GetCoachesAsync();
        if (coachesDb.Count == 0)
        {
            return new List<CoachInfo>();
        }

        var coaches = coachesDb
            .Where(t => t.UserId != _user.Id) // не предлагать пользователю самого себя тренером
            .Select(_mapper.Map<CoachInfo>).ToList();

        foreach (var item in coaches)
        {
            var coachDb = coachesDb.First(t => t.UserId == item.Id);
            item.Name = string.Join(" ", new[] { coachDb.Surname, coachDb.FirstName, coachDb.Patronimic });
        }

        return coaches;
    }

    public class Param { }
}
