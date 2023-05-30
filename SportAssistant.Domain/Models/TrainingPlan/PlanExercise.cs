using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan;

public class PlanExercise : Entity
{
    public int PlanDayId { get; set; }

    public int Order { get; set; }

    public string? Comments { get; set; }

    public string? ExtPlanData { get; set; }

    /// <summary>
    /// Расчетное поле. КПШ по упражнению. Количество Поднятий Штанги
    /// </summary>
    public int LiftCounter { get; set; } = 0;

    /// <summary>
    /// Расчетное поле. Нагрузка. Общий поднятый вес при выполнении упражнения.
    /// </summary>
    public int WeightLoad { get; set; } = 0;

    /// <summary>
    /// Расчетное поле. Интенсивность, это отношение Нагрузки к КПШ. Показывает средний поднимаемый вес.
    /// </summary>
    public int Intensity { get; set; } = 0;

    /// <summary>
    /// Расчетные данные. КПШ по процентной интенсивности занятий.
    /// </summary>
    public List<LiftIntensity>? LiftIntensities { get; set; } = new List<LiftIntensity>();

    public Exercise? Exercise { get; set; } = new Exercise();

    public List<PlanExerciseSettings>? Settings { get; set; } = new List<PlanExerciseSettings>();

    public PlanExerciseSettings SettingsTemplate { get; set; } = new PlanExerciseSettings();

    public OwnerOfPlan Owner { get; set; } = new OwnerOfPlan();
}