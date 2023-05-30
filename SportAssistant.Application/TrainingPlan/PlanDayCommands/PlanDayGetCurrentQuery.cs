using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands;

/// <summary>
/// Получение тренировочного дня из плана на текущий день для вызывающего пользователя.
/// </summary>
public class PlanDayGetCurrentQuery : ICommand<PlanDayGetCurrentQuery.Param, PlanDay>
{
    private readonly IProcessPlanDay _processPlanDay;
    private readonly IUserProvider _user;

    public PlanDayGetCurrentQuery(
        IProcessPlanDay processPlanDay,
        IUserProvider user)
    {
        _processPlanDay = processPlanDay;
        _user = user;
    }

    public async Task<PlanDay> ExecuteAsync(Param param)
    {
        var day = await _processPlanDay.GetCurrentDay(_user.Id);
        return day ?? new PlanDay();
    }

    public class Param
    {
    }
}
