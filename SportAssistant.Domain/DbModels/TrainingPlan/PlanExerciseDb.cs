using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan;


[Table("PlanExercises", Schema = "plan")]
public class PlanExerciseDb : EntityDb
{
    public int PlanDayId { get; set; }

    public int ExerciseId { get; set; }

    public int Order { get; set; }

    public string? Comments { get; set; }

    public string? ExtPlanData { get; set; }
}
