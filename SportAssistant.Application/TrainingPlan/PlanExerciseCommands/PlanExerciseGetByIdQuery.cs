using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Получение запланированного упражнения по его Ид.
    /// </summary>
    public class PlanExerciseGetByIdQuery : ICommand<PlanExerciseGetByIdQuery.Param, PlanExercise>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessPlanUserId _processPlanUserId;
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanExerciseGetByIdQuery(
            IProcessPlan processPlan,
            IProcessPlanUserId processPlanUserId,
            IProcessPlanExercise processPlanExercise,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository)
        {
            _processPlan = processPlan;
            _processPlanUserId = processPlanUserId;
            _processPlanExercise = processPlanExercise;
            _planExerciseRepository = plannedExerciseRepository;
        }

        public async Task<PlanExercise> ExecuteAsync(Param param)
        {
            var planExerciseDb = await _planExerciseRepository.FindOneAsync(t => t.Id == param.Id);
            if (planExerciseDb != null)
            {
                var planUserId = await _processPlanUserId.GetByPlanExerciseId(param.Id);
                await _processPlan.ViewAllowedForDataOfUserAsync(planUserId);
            }
            else
            {
                return new PlanExercise();
            }

            var list = new List<PlanExerciseDb>() { planExerciseDb };
            var exercise = await _processPlanExercise.PrepareExerciseDataAsync(list);
            return exercise.First();
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
