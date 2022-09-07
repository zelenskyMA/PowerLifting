using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{

    [Table("PlanExercises", Schema = "dbo")]
    public class PlanExerciseDb : EntityDb
    {
        public int PlanDayId { get; set; }

        public int ExerciseId { get; set; }

        public int Order { get; set; }

        public int LiftCounter { get; set; } = 0;

        public int WeightLoad { get; set; } = 0;

        public int Intensity { get; set; } = 0;
    }
}
