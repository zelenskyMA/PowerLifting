namespace PowerLifting.Domain.Models.TrainingWork
{
  public class Exercise
  {
    public int Id { get; set; }

    public int ExerciseTypeId { get; set; }

    /// <summary>
    /// Заполняется по ExerciseTypeId
    /// </summary>
    public string Name { get; set; }

    public int Order { get; set; }

    public int LiftCounter { get; set; }

    public int WeightLoad { get; set; }

    public int Intensity { get; set; }

    public List<ExercisePercentage> ExerciseData { get; set; }
  }
}
