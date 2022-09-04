using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    [Table("ExercisePercentages", Schema = "dbo")]
    public class ExercisePercentageDb : EntityDb
    {
        public int PercentageId { get; set; }

        public int PlannedExerciseId { get; set; }

        public int ExerciseSettingsId { get; set; }
    }
}
