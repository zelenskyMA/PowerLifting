using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    [Table("TrainingDays", Schema = "dbo")]
    public class TrainingDayDb : EntityDb
    {
        public int TrainingPlanId { get; set; }

        public DateTime ActivityDate { get; set; }

        public int LiftCounterSum { get; set; } = 0;

        public int WeightLoadSum { get; set; } = 0;

        public int IntensitySum { get; set; } = 0;
    }
}