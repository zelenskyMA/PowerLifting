using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.Coaching
{
    [Table("TrainingRequests", Schema = "trn")]
    public class TrainingRequestDb : EntityDb
    {
        public int UserId { get; set; }

        public int CoachId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
