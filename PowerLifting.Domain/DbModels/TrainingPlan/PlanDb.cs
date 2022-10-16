using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    [Table("Plans", Schema = "plan")]
    public class PlanDb : EntityDb
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public string? Comments { get; set; }
    }
}
