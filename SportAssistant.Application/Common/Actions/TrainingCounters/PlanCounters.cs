using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.Common.Actions.TrainingCounters
{
    public class PlanCounters
    {
        public void SetPlanCounters(Plan plan)
        {
            var listCounters = plan.TrainingDays.SelectMany(t => t.ExerciseTypeCounters).ToList();
            plan.TypeCountersSum = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();
        }

        public void SetTemplatePlanCounters(TemplatePlan plan)
        {
            var listCounters = plan.TrainingDays.SelectMany(t => t.ExerciseTypeCounters).ToList();
            plan.TypeCountersSum = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();
        }

        /// <summary>
        /// Cчитаем, сколько упражнений по подтипам в плане.
        /// </summary>
        /// <returns></returns>
        private List<ValueEntity> CountExerciseTypes(List<ValueEntity> exerciseList)
        {
            var typeCounters = new List<ValueEntity>();
            foreach (var item in exerciseList)
            {
                var dayIntensityItem = typeCounters.FirstOrDefault(t => t.Id == item.Id);
                if (dayIntensityItem == null)
                {
                    dayIntensityItem = new ValueEntity()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Value = 0
                    };
                    typeCounters.Add(dayIntensityItem);
                }

                dayIntensityItem.Value += item.Value;
            }

            return typeCounters;
        }
    }
}
