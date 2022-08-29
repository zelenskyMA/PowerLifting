namespace PowerLifting.Domain.Models.TrainingWork
{
  public class TrainingPlan
  {
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime StartDate { get; set; }

    public string Comments { get; set; }

    public List<TrainingDay> TrainingDays { get; set; }
  }
}
