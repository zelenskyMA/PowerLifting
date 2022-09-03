using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{

  [Table("PlannedExercises", Schema = "dbo")]
  public class PlannedExerciseDb : EntityDb
  {
    public int TrainingDayId { get; set; }

    public int ExerciseId { get; set; }

    public int Order { get; set; }

    public int LiftCounter { get; set; }

    public int WeightLoad { get; set; }

    public int Intensity { get; set; }
  }
}
