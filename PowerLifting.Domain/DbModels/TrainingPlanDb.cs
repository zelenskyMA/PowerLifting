using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{
  [Table("TrainingPlans", Schema = "dbo")]
  public class TrainingPlanDb : EntityModel
  {
    public int UserId { get; set; }

    public DateTime StartDate { get; set; }

    public string Comments { get; set; }
  }
}
