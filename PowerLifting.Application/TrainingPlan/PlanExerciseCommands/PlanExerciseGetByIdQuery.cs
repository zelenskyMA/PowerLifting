using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Получение запланированного упражнения по его Ид.
    /// </summary>
    public class PlanExerciseGetByIdQuery : ICommand<PlanExerciseGetByIdQuery.Param, PlanExercise>
    {
        private readonly IProcessPlanExercise _processPlanExercise;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanExerciseGetByIdQuery(
            IProcessPlanExercise processPlanExercise,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository)
        {
            _processPlanExercise = processPlanExercise;
            _planExerciseRepository = plannedExerciseRepository;
        }

        public async Task<PlanExercise> ExecuteAsync(Param param)
        {
            var planExerciseDb = await _planExerciseRepository.FindAsync(t => t.Id == param.Id);
            var exercise = (await _processPlanExercise.PrepareExerciseDataAsync(planExerciseDb)).FirstOrDefault() ?? new PlanExercise();
            return exercise;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
