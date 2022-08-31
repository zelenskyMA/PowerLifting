using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{
  [Table("TrainingDays", Schema = "dbo")]
  public class TrainingDayDb : EntityDb
  {
    public int TrainingPlanId { get; set; }

    public DateTime ActivityDate { get; set; }

    public int LiftCounterSum { get; set; }

    public int WeightLoadSum { get; set; }

    public int IntensitySum { get; set; }
  }
}