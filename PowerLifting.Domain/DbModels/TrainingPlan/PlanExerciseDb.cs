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
    }
}
