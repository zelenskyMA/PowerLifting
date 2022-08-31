using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{

  [Table("Exercises", Schema = "dbo")]
  public class ExerciseDb : EntityModel
  {
    public int TrainingDayId { get; set; }

    public int ExerciseTypeId { get; set; }

    public int Order { get; set; }

    public int LiftCounter { get; set; }

    public int WeightLoad { get; set; }

    public int Intensity { get; set; }
  }
}
