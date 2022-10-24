using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Models.TraininTemplate
{
    public class TemplateDay : Entity
    {
        public int TemplatePlanId { get; set; }

        public int DayNumber { get; set; }

        /// <summary>
        /// Расчетное поле. Сумма КПШ по всем упражнениям за день.
        /// </summary>
        public int LiftCounterSum { get; set; }

        /// <summary>
        /// Расчетное поле. Суммарный процент Нагрузок по всем упражнениям за день.
        /// </summary>
        public int WeightLoadPercentageSum { get; set; }

        /// <summary>
        /// Расчетные данные. Количество упражнений по подтипам.
        /// </summary>
        public List<ValueEntity> ExerciseTypeCounters { get; set; } = new List<ValueEntity>();

        /// <summary>
        /// Упражнения, назначенные на тренировочный день.
        /// </summary>
        public List<TemplateExercise>? Exercises { get; set; } = new List<TemplateExercise>();

        /// <summary>
        /// Процентовки, которые задействованы в плане на день
        /// </summary>
        public List<Percentage> Percentages { get; set; }
    }
}
