namespace PowerLifting.Service
{
  public class WorkDay
  {
    public string Name { get; set; }

    public int ExerciseCount { get; set; }

    public List<ExerciseData> Data { get; set; }
  }
}
