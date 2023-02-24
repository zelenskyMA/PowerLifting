using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingTemplate
{
    [Table("TemplateDays", Schema = "plan")]
    public class TemplateDayDb : EntityDb
    {
        public int TemplatePlanId { get; set; }

        public int DayNumber { get; set; }
    }
}