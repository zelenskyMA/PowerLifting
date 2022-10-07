using PowerLifting.Domain.Interfaces.TrainingPlan.Application.Process;
using PowerLifting.Domain.Models.Common;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.Process
{
    public class PlanCountersSetup : IPlanCountersSetup
    {
        /// <inheritdoc />
        public void SetPlanCounters(Plan plan)
        {
            var planCounters = new List<ValueEntity>();

            var listCounters = plan.TrainingDays.Select(t => t.ExerciseTypeCounters).ToList();
            foreach (var itemList in listCounters)
            {
                foreach (var item in itemList)
                {
                    var dayIntensityItem = planCounters.FirstOrDefault(t => t.Id == item.Id);
                    if (dayIntensityItem == null)
                    {
                        dayIntensityItem = new ValueEntity()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Value = 0
                        };
                        planCounters.Add(dayIntensityItem);
                    }

                    dayIntensityItem.Value += item.Value;
                }
            }

            plan.TypeCountersSum = planCounters.OrderBy(t => t.Name).ToList();
        }

        /// <inheritdoc />
        public void SetPlanDayCounters(PlanDay day)
        {
            if (day.Exercises == null || day.Exercises.Count == 0)
            {
                return;
            }

            // простые суммы значений
            day.WeightLoadSum = day.Exercises.Sum(t => t.WeightLoad);
            day.LiftCounterSum = day.Exercises.Sum(t => t.LiftCounter);
            day.IntensitySum = day.Exercises.Count() > 0 ? day.Exercises.Sum(t => t.Intensity) / day.Exercises.Count() : 0;


            // считаем, сколько упражнений по подтипам в тренировочном дне. 
            day.ExerciseTypeCounters = new List<ValueEntity>();
            var groups = day.Exercises.Select(t => t.Exercise).GroupBy(t => t.ExerciseSubTypeId);
            foreach (var item in groups)
            {
                day.ExerciseTypeCounters.Add(new ValueEntity()
                {
                    Id = item.Select(t => t.ExerciseSubTypeId).First(),
                    Name = item.Select(t => t.ExerciseSubTypeName).First(),
                    Value = item.Count()
                });
            }
            day.ExerciseTypeCounters = day.ExerciseTypeCounters.OrderBy(t => t.Name).ToList();


            // считаем дневную интенсивность занятий по колонкам процентов.
            var listIntensities = day.Exercises.Select(t => t.LiftIntensities).ToList();
            var dayIntensities = listIntensities.First();
            listIntensities.RemoveAt(0);
            foreach (var itemList in listIntensities)
            {
                foreach (var item in itemList)
                {
                    var dayIntensityItem = dayIntensities.FirstOrDefault(t => t.Percentage.Id == item.Percentage.Id);
                    dayIntensityItem.Value += item.Value;
                }
            }

            day.LiftIntensities = dayIntensities.OrderBy(t => t.Percentage.MinValue).ToList();
        }

        /// <inheritdoc />
        public void SetPlanExerciseCounters(PlanExercise planExercise)
        {
            if (planExercise.Settings == null || planExercise.Settings.Count == 0)
            {
                return;
            }

            planExercise.WeightLoad = planExercise.Settings.Select(
                t => t.Weight * t.Iterations * (t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3)).Sum();

            planExercise.LiftCounter = planExercise.Settings.Select(
                t => t.ExercisePart1 + t.ExercisePart2 + t.ExercisePart3).Sum();

            planExercise.Intensity = planExercise.LiftCounter == 0 ? 0 : planExercise.WeightLoad / planExercise.LiftCounter;

            planExercise.LiftIntensities = new List<LiftIntensity>();
            foreach (var item in planExercise.Settings)
            {
                planExercise.LiftIntensities.Add(new LiftIntensity()
                {
                    Percentage = item.Percentage,
                    Value = item.ExercisePart1 + item.ExercisePart2 + item.ExercisePart3,
                });
            };
        }
    }
}
