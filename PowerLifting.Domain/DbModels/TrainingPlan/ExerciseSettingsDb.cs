using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    [Table("ExerciseSettings", Schema = "dbo")]
    public class ExerciseSettingsDb : EntityDb
    {
        public int Weight { get; set; }

        public int Iterations { get; set; }

        public int ExercisePart1 { get; set; }

        public int ExercisePart2 { get; set; }

        public int ExercisePart3 { get; set; }

        public string Comments { get; set; }
    }
}
