using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.Common.Actions.TrainingCounters;

public class TrainingCountersSetup : ITrainingCountersSetup
{
    private readonly PlanCounters _planCounters;
    private readonly DayCounters _dayCounters;
    private readonly ExerciseCounters _exerciseCounters;

    public TrainingCountersSetup()
    {
        _planCounters = new PlanCounters();
        _dayCounters = new DayCounters();
        _exerciseCounters = new ExerciseCounters();
    }

    /// <inheritdoc />
    public void SetPlanCounters(Plan plan) => _planCounters.SetPlanCounters(plan);

    /// <inheritdoc />
    public void SetPlanCounters(TemplatePlan plan) => _planCounters.SetTemplatePlanCounters(plan);

    /// <inheritdoc />
    public void SetDayCounters(PlanDay day) => _dayCounters.SetPlanDayCounters(day);

    /// <inheritdoc />
    public void SetDayCounters(TemplateDay day) => _dayCounters.SetTemplateDayCounters(day);

    /// <inheritdoc />
    public void SetExerciseCounters(PlanExercise exercise) => _exerciseCounters.SetPlanExerciseCounters(exercise);

    /// <inheritdoc />
    public void SetExerciseCounters(TemplateExercise exercise) => _exerciseCounters.SetTemplateExerciseCounters(exercise);
}
