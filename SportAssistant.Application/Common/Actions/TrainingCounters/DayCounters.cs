using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportAssistant.Application.Common.Actions.TrainingCounters
{
    public class DayCounters
    {
        public void SetPlanDayCounters(PlanDay day)
        {
            if (day.Exercises == null || day.Exercises.Count == 0)
            {
                return;
            }

            day.WeightLoadSum = day.Exercises.Sum(t => t.WeightLoad);
            day.LiftCounterSum = day.Exercises.Sum(t => t.LiftCounter);
            day.IntensitySum = ExerciseIntensitySum(day.Exercises);
            day.ExerciseTypeCounters = CountExerciseTypes(day.Exercises.Select(t => t.Exercise));
            day.LiftIntensities = PercentageIntensities(day.Exercises);
        }

        public void SetTemplateDayCounters(TemplateDay day)
        {
            if (day.Exercises == null || day.Exercises.Count == 0)
            {
                return;
            }

            day.WeightLoadPercentageSum = day.Exercises.Sum(t => t.WeightLoadPercentage);
            day.LiftCounterSum = day.Exercises.Sum(t => t.LiftCounter);
            day.ExerciseTypeCounters = CountExerciseTypes(day.Exercises.Select(t => t.Exercise));
        }

        /// <summary> Cчитаем, сколько упражнений по подтипам в тренировочном дне. </summary>
        private List<ValueEntity> CountExerciseTypes(IEnumerable<Exercise> exercises)
        {
            var typeCounters = new List<ValueEntity>();
            var groups = exercises.GroupBy(t => t.ExerciseSubTypeId);
            foreach (var item in groups)
            {
                typeCounters.Add(new ValueEntity()
                {
                    Id = item.Select(t => t.ExerciseSubTypeId).First(),
                    Name = item.Select(t => t.ExerciseSubTypeName).First(),
                    Value = item.Count()
                });
            }

            return typeCounters.OrderBy(t => t.Name).ToList();
        }

        /// <summary> Расчет интенсивности по упражнению. Горизонтальная интенсивность </summary>
        private int ExerciseIntensitySum(IEnumerable<PlanExercise> exercises)
        {
            if (exercises == null || exercises.Count() == 0)
            {
                return 0;
            }

            return exercises.Sum(t => t.Intensity) / exercises.Count();
        }

        /// <summary> Расчет интенсивности по процетовке. Вертикальная интенсивность </summary>
        private List<LiftIntensity> PercentageIntensities(List<PlanExercise> exercises)
        {
            var groupedIntensities = exercises.Where(t => t.LiftIntensities != null && t.LiftIntensities.Count > 0)
               .SelectMany(t => t.LiftIntensities)
               .GroupBy(t => t.Percentage.Id).ToList();

            var dayIntensities = new List<LiftIntensity>();
            foreach (var group in groupedIntensities)
            {
                dayIntensities.Add(new LiftIntensity()
                {
                    Percentage = group.First().Percentage,
                    Value = group.Sum(t => t.Value)
                });
            }

            return dayIntensities.OrderBy(t => t.Percentage.MinValue).ToList();
        }
    }
}
