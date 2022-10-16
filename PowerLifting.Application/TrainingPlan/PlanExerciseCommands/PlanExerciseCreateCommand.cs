using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.PlanExerciseCommands
{
    /// <summary>
    /// Планирование сета упражнений в рамках тренировочного дня.
    /// </summary>
    public class PlanExerciseCreateCommand : ICommand<PlanExerciseCreateCommand.Param, bool>
    {
        private readonly IProcessPlanExerciseSettings _processPlanExerciseSettings;
        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;

        public PlanExerciseCreateCommand(
            IProcessPlanExerciseSettings processPlanExerciseSettings,
            ICrudRepo<PlanExerciseDb> plannedExerciseRepository)
        {
            _processPlanExerciseSettings = processPlanExerciseSettings;
            _planExerciseRepository = plannedExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            if (param.Exercises.Count == 0)
            {
                return false;
            }

            //удаляем лишние записи вместе со связями
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == param.DayId);
            if (planExercisesDb.Count > 0)
            {
                var itemsToDelete = planExercisesDb.Where(t => !param.Exercises.Select(t => t.PlannedExerciseId).Contains(t.Id)).ToList();
                if (itemsToDelete.Count > 0)
                {
                    await _processPlanExerciseSettings.DeleteByPlanExerciseIdAsync(itemsToDelete.Select(t => t.Id).ToList());
                    _planExerciseRepository.DeleteList(itemsToDelete);

                    foreach (var item in itemsToDelete)
                    {
                        planExercisesDb.Remove(item);
                    }
                }
            }

            for (int i = 1; i <= param.Exercises.Count; i++)
            {
                // обновление существующего упражнения
                var planExercise = planExercisesDb.FirstOrDefault(t => t.Id == param.Exercises[i - 1].PlannedExerciseId);
                if (planExercise != null)
                {
                    planExercise.Order = i;
                    _planExerciseRepository.Update(planExercise);
                    continue;
                }

                // добавление нового упражнения
                planExercise = new PlanExerciseDb()
                {
                    PlanDayId = param.DayId,
                    ExerciseId = param.Exercises[i - 1].Id,
                    Order = i
                };

                await _planExerciseRepository.CreateAsync(planExercise);
            }

            return true;
        }

        public class Param
        {
            public int DayId { get; set; }

            public List<Exercise> Exercises { get; set; }
        }
    }
}