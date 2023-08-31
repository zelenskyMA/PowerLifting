using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands;

/// <summary>
/// Получение тренировочного дня из плана по Ид.
/// </summary>
public class PlanDayGetByIdQuery : ICommand<PlanDayGetByIdQuery.Param, PlanDay>
{
    private readonly IProcessPlanDay _processPlanDay;
    private readonly IProcessPlan _processPlan;
    private readonly IProcessPlanUserId _processPlanUserId;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IUserProvider _user;

    public PlanDayGetByIdQuery(
        IProcessPlanDay processPlanDay,
        IProcessPlan processPlan,
        IProcessPlanUserId processPlanUserId,
        IProcessUserInfo processUserInfo,
        IUserProvider user)
    {
        _processPlanDay = processPlanDay;
        _processPlan = processPlan;
        _processPlanUserId = processPlanUserId;
        _processUserInfo = processUserInfo;
        _user = user;
    }

    public async Task<PlanDay> ExecuteAsync(Param param)
    {
        var planDay = await _processPlanDay.GetAsync(param.Id);
        if (planDay == null)
        {
            return planDay;
        }

        var planUserId = await _processPlanUserId.GetByDayIdAsync(param.Id);
        await _processPlan.ViewAllowedForDataOfUserAsync(planUserId);
        
        if (planUserId != _user.Id)
        {
            var info = await _processUserInfo.GetInfo(planUserId);
            planDay.Owner.Name = info.LegalName;
        }
        
        return planDay;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
