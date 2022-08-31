using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingWork
{
  public class TrainingDay : Entity
  {
    public DateTime ActivityDate { get; set; }

    public int LiftCounterSum { get; set; }

    public int WeightLoadSum { get; set; }

    public int IntensitySum { get; set; }

    public List<PlannedExercise> Exercises { get; set; }
  }
}
