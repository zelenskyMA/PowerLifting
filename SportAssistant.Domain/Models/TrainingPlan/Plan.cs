using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan
{
    public class Plan : Entity
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsMyPlan { get; set; }

        /// <summary>
        /// Расчетные данные. Количество упражнений по подтипам в сумме за все дни плана.
        /// </summary>
        public List<ValueEntity> TypeCountersSum { get; set; } = new List<ValueEntity>();

        public List<PlanDay> TrainingDays { get; set; } = new List<PlanDay>();

        public DateTime FinishDate => StartDate.Date.AddDays(6);
    }
}
