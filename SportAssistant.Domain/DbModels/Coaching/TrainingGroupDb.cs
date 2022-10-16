using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Coaching
{
    [Table("TrainingGroups", Schema = "trn")]
    public class TrainingGroupDb : EntityDb
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int CoachId { get; set; }
    }
}
