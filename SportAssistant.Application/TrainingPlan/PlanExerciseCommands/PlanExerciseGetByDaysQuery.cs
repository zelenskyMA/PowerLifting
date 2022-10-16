using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Получение запланированных упражнений по Ид дней в плане.
    /// </summary>
    public class PlanExerciseGetByDaysQuery : ICommand<PlanExerciseGetByDaysQuery.Param, List<PlanExercise>>
    {
        private readonly IProcessPlanExercise _processPlanExercise;

        public PlanExerciseGetByDaysQuery(
            IProcessPlanExercise processPlanExercise)
        {
            _processPlanExercise = processPlanExercise;
        }

        public async Task<List<PlanExercise>> ExecuteAsync(Param param)
        {
            var exercises = await _processPlanExercise.GetByDaysAsync(param.DayIds);
            return exercises;
        }

        public class Param
        {
            public List<int> DayIds { get; set; }
        }
    }
}
