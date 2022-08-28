namespace PowerLifting.Domain.Models.TrainingWork
{
  public class WorkDay
  {
    public int Id { get; set; }

    public DateTime ActivityDate { get; set; }

    public int LiftCounterSum { get; set; }

    public int WeightLoadSum { get; set; }

    public int IntensitySum { get; set; }

    public List<Exercise> Exercises { get; set; }
  }
}
