using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application;

/// <summary>
/// Декоратор для осуществления расчетов по планам и шаблонам пользователя
/// </summary>
public interface ITrainingCountersSetup
{
    /// <summary>
    /// Установка расчетных данных на уровне плана
    /// </summary>
    /// <param name="plan">Тренировочный план</param>
    void SetPlanCounters(Plan plan);

    /// <summary>
    /// Установка расчетных данных на уровне шаблона плана
    /// </summary>
    /// <param name="plan">Шаблон</param>
    void SetPlanCounters(TemplatePlan plan);

    /// <summary>
    /// Установка расчетных данных на уровне тренировочного дня
    /// </summary>
    /// <param name="day">тренировочный день</param>
    void SetDayCounters(PlanDay day);

    /// <summary>
    /// Установка расчетных данных на уровне шаблона дня
    /// </summary>
    /// <param name="day">шаблон дня</param>
    void SetDayCounters(TemplateDay day);

    /// <summary>
    /// Установка расчетных данных на уровне запланированного упражнения
    /// </summary>
    /// <param name="planExercise">Запланированное упражнение</param>
    void SetExerciseCounters(PlanExercise planExercise);

    /// <summary>
    /// Установка расчетных данных на уровне шаблона запланированного упражнения
    /// </summary>
    /// <param name="exercise">Шаблон запланированного упражнения</param>
    void SetExerciseCounters(TemplateExercise exercise);
}
