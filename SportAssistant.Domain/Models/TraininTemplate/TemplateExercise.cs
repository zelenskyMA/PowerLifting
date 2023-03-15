using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Domain.Models.TrainingTemplate
{
    public class TemplateExercise : Entity
    {
        public int TemplateDayId { get; set; }

        public int Order { get; set; }

        public string? Comments { get; set; }

        public string? ExtPlanData { get; set; }

        /// <summary>
        /// Расчетное поле. КПШ по упражнению. Количество Поднятий Штанги
        /// </summary>
        public int LiftCounter { get; set; } = 0;

        /// <summary>
        /// Расчетное поле. Нагрузка. Процент общего поднятого веса при выполнении упражнения.
        /// </summary>
        public int WeightLoadPercentage { get; set; } = 0;

        public Exercise? Exercise { get; set; } = new Exercise();

        public List<TemplateExerciseSettings>? Settings { get; set; } = new List<TemplateExerciseSettings>();

        public TemplateExerciseSettings SettingsTemplate { get; set; } = new TemplateExerciseSettings();
    }
}
