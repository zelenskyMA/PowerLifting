namespace PowerLifting.Domain.Models.TrainingWork
{
  public class ExerciseValue
  {
    public int Id { get; set; }

    public int Weight { get; set; }

    public int Iterations { get; set; }

    public int ExercisePart1 { get; set; }

    public int ExercisePart2 { get; set; }

    public int ExercisePart3 { get; set; }

    public string Comments { get; set; }
  }
}
