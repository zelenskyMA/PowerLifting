using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingWork
{
  public class Exercise : NamedEntity
  {
    public int ExerciseTypeId { get; set; }

    public string ExerciseTypeName { get; set; }
  }
}
