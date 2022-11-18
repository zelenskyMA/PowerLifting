using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan
{
    [Table("PlanDays", Schema = "plan")]
    public class PlanDayDb : EntityDb
    {
        public int PlanId { get; set; }

        public DateTime ActivityDate { get; set; }
    }
}