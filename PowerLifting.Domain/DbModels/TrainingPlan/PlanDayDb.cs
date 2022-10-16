using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    [Table("PlanDays", Schema = "plan")]
    public class PlanDayDb : EntityDb
    {
        public int PlanId { get; set; }

        public DateTime ActivityDate { get; set; }

        public int LiftCounterSum { get; set; } = 0;

        public int WeightLoadSum { get; set; } = 0;

        public int IntensitySum { get; set; } = 0;
    }
}