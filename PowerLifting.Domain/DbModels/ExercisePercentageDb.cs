using PowerLifting.Domain.DbModels.Common;

namespace PowerLifting.Domain.DbModels
{
  public class ExercisePercentageDb : EntityDb
  {
    public int PercentageId { get; set; }

    public int PlannedExerciseId { get; set; }

    public int ExerciseSettingsId { get; set; }
  }
}
