using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Получение запланированных упражнений по Ид дней в плане.
    /// </summary>
    public class PlanExerciseGetByDayQuery : ICommand<PlanExerciseGetByDayQuery.Param, List<PlanExercise>>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanUserId _processPlanUserId;
        private readonly IProcessPlanExercise _processPlanExercise;

        public PlanExerciseGetByDayQuery(
            IProcessPlan processPlan,
            IProcessPlanUserId processPlanUserId,
            IProcessPlanExercise processPlanExercise)
        {
            _processPlan = processPlan;
            _processPlanUserId = processPlanUserId;
            _processPlanExercise = processPlanExercise;
        }

        public async Task<List<PlanExercise>> ExecuteAsync(Param param)
        {
            var exercises = await _processPlanExercise.GetByDaysAsync(new List<int>() { param.DayId });

            if (exercises.Count > 0) //запрет просмотра чужих данных
            {
                var userId = await _processPlanUserId.GetByDayId(param.DayId);
                await _processPlan.ViewAllowedForDataOfUserAsync(userId);
            }

            return exercises;
        }

        public class Param
        {
            public int DayId { get; set; }
        }
    }
}
