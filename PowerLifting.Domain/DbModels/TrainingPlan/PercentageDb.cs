using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    [Table("Percentages", Schema = "dbo")]
    public class PercentageDb : NamedEntityDb
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}
