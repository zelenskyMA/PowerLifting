using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;

namespace SportAssistant.Application.TrainingPlan.PlanDayCommands;

/// <summary>
/// Перенос упражнений на другой день.
/// </summary>
public class PlanDayMoveCommand : ICommand<PlanDayMoveCommand.Param, bool>
{
    private readonly IProcessPlan _processPlan;
    private readonly IProcessPlanExercise _processPlanExercise;
    private readonly IProcessPlanUserId _processPlanUserId;
    private readonly ICrudRepo<PlanDayDb> _planDayRepository;
    private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

    public PlanDayMoveCommand(
        IProcessPlan processPlan,
        IProcessPlanExercise processPlanExercise,
        IProcessPlanUserId processPlanUserId,
        ICrudRepo<PlanDayDb> planDayRepository,
        ICrudRepo<PlanExerciseDb> planExerciseRepository)
    {
        _processPlan = processPlan;
        _processPlanExercise = processPlanExercise;
        _processPlanUserId = processPlanUserId;
        _planDayRepository = planDayRepository;
        _planExerciseRepository = planExerciseRepository;
    }

    public async Task<bool> ExecuteAsync(Param param)
    {
        if (param.Id == 0 || param.PlanId == 0 || param.TargetDate == null)
        {
            throw new BadRequestException("Не задан один из обязательных параметров.");
        }

        var oldExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == param.Id);
        if (oldExercisesDb.Count == 0)
        {
            return false;
        }

        var oldDay = await _planDayRepository.FindOneAsync(t => t.Id == param.Id && t.PlanId == param.PlanId);
        if (oldDay == null)
        {
            throw new BusinessException("Редактируемый день не входит в указанный план или не существует.");
        }

        var newPlanDay = await _planDayRepository.FindOneAsync(t => t.PlanId == param.PlanId && t.ActivityDate == param.TargetDate);
        if (newPlanDay == null)
        {
            throw new BusinessException("В вашем текущем тренировочном плане нет указанной даты");
        }

        await GetAndCheckUserId(newPlanDay.Id);

        var newExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == newPlanDay.Id);
        await _processPlanExercise.DeletePlanExercisesAsync(newExercisesDb);

        foreach (var item in oldExercisesDb)
        {
            item.PlanDayId = newPlanDay.Id;
        }

        _planExerciseRepository.UpdateList(oldExercisesDb);

        return true;
    }

    private async Task<int> GetAndCheckUserId(int dayId)
    {
        var userId = await _processPlanUserId.GetByDayIdAsync(dayId);
        return await _processPlan.PlanningAllowedForUserAsync(userId);
    }

    public class Param
    {
        public int PlanId { get; set; }

        public int Id { get; set; }

        public DateTime? TargetDate { get; set; }
    }
}
