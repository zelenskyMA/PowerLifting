using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class PlanDay : Entity
    {
        /// <summary>
        /// Дата тренировочного дня.
        /// </summary>
        public DateTime ActivityDate { get; set; }

        /// <summary>
        /// Расчетное поле. Сумма КПШ по всем упражнениям за день.
        /// </summary>
        public int LiftCounterSum { get; set; }

        /// <summary>
        /// Расчетное поле. Сумма Нагрузок по всем упражнениям за день.
        /// </summary>
        public int WeightLoadSum { get; set; }

        /// <summary>
        /// Расчетное поле. Сумма Интенсивности по всем упражнениям за день.
        /// </summary>
        public int IntensitySum { get; set; }

        /// <summary>
        /// Расчетные данные. Сумма КПШ по процентной интенсивности занятий.
        /// </summary>
        public List<LiftIntensity> LiftIntensities { get; set; } = new List<LiftIntensity>();

        /// <summary>
        /// Расчетные данные. Количество упражнений по подтипам.
        /// </summary>
        public List<NamedEntity> ExerciseTypeCounters { get; set; } = new List<NamedEntity>();


        /// <summary>
        /// Упражнения, назначенные на тренировочный день.
        /// </summary>
        public List<PlanExercise>? Exercises { get; set; } = new List<PlanExercise>();
    }
}
