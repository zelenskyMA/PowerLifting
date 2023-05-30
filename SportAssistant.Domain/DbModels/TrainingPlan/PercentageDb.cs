using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan;

[Table("Percentages", Schema = "plan")]
public class PercentageDb : NamedEntityDb
{
    public int MinValue { get; set; } = 0;

    public int MaxValue { get; set; } = 0;
}
