using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IPlanCountersSetup
    {
        /// <summary>
        /// Count and set cumulative values in plan
        /// </summary>
        /// <param name="plan">Training plan</param>
        void SetPlanCounters(Plan plan);

        /// <summary>
        /// Count and set cumulative values in plan day.
        /// </summary>
        /// <param name="day">Training plan day</param>
        void SetPlanDayCounters(PlanDay day);

        /// <summary>
        /// Count and set cumulative values in planned exercise.
        /// </summary>
        /// <param name="planExercise">Planned exercise</param>
        void SetPlanExerciseCounters(PlanExercise planExercise);
    }
}
