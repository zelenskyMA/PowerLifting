using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.Common.Actions.TrainingCounters;

public class ExerciseCounters
{
    public void SetPlanExerciseCounters(PlanExercise exercise)
    {
        if (exercise.Settings == null || exercise.Settings.Count == 0)
        {
            return;
        }

        exercise.WeightLoad = exercise.Settings.Sum(t => t.Weight * t.Iterations * (t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3));
        exercise.LiftCounter = exercise.Settings.Sum(t => t.Iterations * (t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3));
        exercise.Intensity = exercise.LiftCounter == 0 ? 0 : exercise.WeightLoad / exercise.LiftCounter;

        exercise.LiftIntensities = ExerciseIntensity(exercise.Settings);
    }

    public void SetTemplateExerciseCounters(TemplateExercise exercise)
    {
        if (exercise.Settings == null || exercise.Settings.Count == 0)
        {
            return;
        }

        exercise.LiftCounter = exercise.Settings.Sum(t => t.Iterations * (t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3));
        exercise.WeightLoadPercentage = exercise.Settings.Sum(t => t.WeightPercentage);
    }

    /// <summary> Расчет интенсивности по упражнению. Горизонтальная интенсивность </summary>
    private List<LiftIntensity> ExerciseIntensity(List<PlanExerciseSettings> settings)
    {
        var liftIntensities = new List<LiftIntensity>();
        foreach (var item in settings)
        {
            liftIntensities.Add(new LiftIntensity()
            {
                Percentage = item.Percentage,
                Value = item.ExercisePart1 + item.ExercisePart2 + item.ExercisePart3,
            });
        };

        return liftIntensities;
    }
}
