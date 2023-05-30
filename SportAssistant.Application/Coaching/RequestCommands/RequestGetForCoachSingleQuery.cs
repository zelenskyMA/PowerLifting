using AutoMapper;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingRequestCommands;

/// <summary>
/// Получение тренером поданой ему заявки по Ид.
/// </summary>
public class RequestGetForCoachSingleQuery : ICommand<RequestGetForCoachSingleQuery.Param, TrainingRequest>
{
    private readonly ITrainingRequestRepository _trainingRequestRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public RequestGetForCoachSingleQuery(
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
        request.UserName = UserNaming.GetLegalFullName(userInfoDb.FirstName, userInfoDb.Surname, userInfoDb.Patronimic);
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
