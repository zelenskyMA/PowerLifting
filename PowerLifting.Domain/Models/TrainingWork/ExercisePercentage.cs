using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingWork
{
  public class ExercisePercentage : Entity
  {
    public Percentage Percentage { get; set; }

    public ExerciseValue Values { get; set; }

  }
}
