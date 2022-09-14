using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class Plan : Entity
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; } = DateTime.Now;

        public string? Comments { get; set; }

        /// <summary>
        /// Расчетные данные. Количество упражнений по подтипам в сумме за все дни плана.
        /// </summary>
        public List<ValueEntity> TypeCountersSum { get; set; } = new List<ValueEntity>();

        public List<PlanDay>? TrainingDays { get; set; } = new List<PlanDay>();
    }
}
