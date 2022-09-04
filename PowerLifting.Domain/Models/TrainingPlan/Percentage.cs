using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
  public class Percentage : NamedEntity
  {
    public int MinValue { get; set; } = 0;

    public int MaxValue { get; set; } = 0;
  }
}
