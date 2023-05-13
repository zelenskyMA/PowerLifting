using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.Common.Actions.TrainingCounters;

public class PlanCounters
{
    public void SetPlanCounters(Plan plan)
    {
        plan.Counters.WeightLoadSum = plan.TrainingDays.Sum(t => t.Counters.WeightLoadSum);
        plan.Counters.LiftCounterSum = plan.TrainingDays.Sum(t => t.Counters.LiftCounterSum);
        plan.Counters.IntensitySum = plan.TrainingDays.Sum(t => t.Counters.IntensitySum);

        var listCounters = plan.TrainingDays.SelectMany(t => t.Counters.ExerciseTypeCounters).ToList();
        plan.Counters.CategoryCountersSum = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();

        listCounters = plan.TrainingDays.SelectMany(t => t.Counters.WeightLoadsByCategory).ToList();
        plan.Counters.WeightLoadsByCategory = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();

        listCounters = plan.TrainingDays.SelectMany(t => t.Counters.IntensitiesByCategory).ToList();
        plan.Counters.IntensitiesByCategory = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();

        listCounters = plan.TrainingDays.SelectMany(t => t.Counters.LiftCountersByCategory).ToList();
        plan.Counters.LiftCountersByCategory = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();
    }

    public void SetTemplatePlanCounters(TemplatePlan plan)
    {
        var listCounters = plan.TrainingDays.SelectMany(t => t.Counters.ExerciseTypeCounters).ToList();
        plan.Counters.CategoryCountersSum = CountExerciseTypes(listCounters).OrderBy(t => t.Name).ToList();
    }

    /// <summary>
    /// Суммирование данных по дням в плане. 
    /// </summary>
    /// <returns></returns>
    private List<ValueEntity> CountExerciseTypes(List<ValueEntity> dataList)
    {
        var resultCounters = new List<ValueEntity>();
        foreach (var kvPair in dataList)
        {
            var resultKvItem = resultCounters.FirstOrDefault(t => t.Id == kvPair.Id);
            if (resultKvItem == null)
            {
                resultKvItem = new ValueEntity()
                {
                    Id = kvPair.Id,
                    Name = kvPair.Name,
                    Value = 0
                };
                resultCounters.Add(resultKvItem);
            }

            resultKvItem.Value += kvPair.Value;
        }

        return resultCounters;
    }
}
