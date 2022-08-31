namespace PowerLifting.Domain.Models.TrainingWork
{
  public class Percentage
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int MinValue { get; set; } = 0;

    public int MaxValue { get; set; } = 0;
  }
}
