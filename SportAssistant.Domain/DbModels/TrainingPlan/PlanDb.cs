using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan;

[Table("Plans", Schema = "plan")]
public class PlanDb : EntityDb
{
    public int UserId { get; set; }

    public DateTime StartDate { get; set; }
}
