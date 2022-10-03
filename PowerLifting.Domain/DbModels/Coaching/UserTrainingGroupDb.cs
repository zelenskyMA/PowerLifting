using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.Coaching
{
    [Table("UserTrainingGroups", Schema = "trn")]
    public class UserTrainingGroupDb
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }
    }
}