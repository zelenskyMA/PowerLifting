namespace PowerLifting.Domain.Models.TrainingWork
{
  public class WorkPlan
  {
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime StartDate { get; set; }

    public string Comments { get; set; }

    public List<WorkDay> WorkDays { get; set; }
  }
}
