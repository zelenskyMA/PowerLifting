using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands;

/// <summary>
/// Получение запланированного упражнения по его Ид.
/// </summary>
public class PlanExerciseGetByIdQuery : ICommand<PlanExerciseGetByIdQuery.Param, PlanExercise>
{
    private readonly IProcessPlan _processPlan;
    private readonly IProcessPlanUserId _processPlanUserId;
    private readonly IProcessPlanExercise _processPlanExercise;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
    private readonly IUserProvider _user;

    public PlanExerciseGetByIdQuery(
        IProcessPlan processPlan,
        IProcessPlanUserId processPlanUserId,
        IProcessPlanExercise processPlanExercise,
        IProcessUserInfo processUserInfo,
        ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
        IUserProvider user)
    {
        _processPlan = processPlan;
        _processPlanUserId = processPlanUserId;
        _processPlanExercise = processPlanExercise;
        _processUserInfo = processUserInfo;
        _planExerciseRepository = plannedExerciseRepository;
        _user = user;
    }

    public async Task<PlanExercise> ExecuteAsync(Param param)
    {
        var planUserId = 0;
        var planExerciseDb = await _planExerciseRepository.FindOneAsync(t => t.Id == param.Id);
        if (planExerciseDb != null)
        {
            planUserId = await _processPlanUserId.GetByPlanExerciseId(param.Id);
            await _processPlan.ViewAllowedForDataOfUserAsync(planUserId);
        }
        else
        {
            return new PlanExercise();
        }

        var list = new List<PlanExerciseDb>() { planExerciseDb };
        var exercise = (await _processPlanExercise.PrepareExerciseDataAsync(list)).First();

        if (planUserId != _user.Id)
        {
            var info = await _processUserInfo.GetInfo(planUserId);
            exercise.Owner.Name = info.LegalName;
        }

        return exercise;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
