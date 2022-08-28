namespace PowerLifting.Domain.Models.TrainingWork
{
  public class ExercisePercentage
  {
    public int Id { get; set; }

    public Percentage Percentage { get; set; }

    public ExerciseValue Values { get; set; }

  }
}
