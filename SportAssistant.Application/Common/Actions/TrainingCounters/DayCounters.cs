using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;

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

            day.Counters.WeightLoadSum = day.Exercises.Sum(t => t.WeightLoad);
            day.Counters.LiftCounterSum = day.Exercises.Sum(t => t.LiftCounter);
            day.Counters.IntensitySum = day.Exercises.Sum(t => t.Intensity) / day.Exercises.Count();
            day.Counters.LiftIntensities = PercentageIntensities(day.Exercises);

            day.Counters.ExerciseTypeCounters = SumExerciseCategories(day.Exercises.Select(t => t.Exercise));

            SumExerciseCounters(day);
        }

        public void SetTemplateDayCounters(TemplateDay day)
        {
            if (day.Exercises == null || day.Exercises.Count == 0)
            {
                return;
            }

            day.Counters.WeightLoadPercentageSum = day.Exercises.Sum(t => t.WeightLoadPercentage);
            day.Counters.LiftCounterSum = day.Exercises.Sum(t => t.LiftCounter);
            day.Counters.ExerciseTypeCounters = SumExerciseCategories(day.Exercises.Select(t => t.Exercise));
        }

        /// <summary> Суммирование данных по категориям упражнений в тренировочном дне. </summary>
        private List<ValueEntity> SumExerciseCategories(IEnumerable<Exercise> exercises)
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

        /// <summary> Суммирование данных по категориям упражнений в тренировочном дне. </summary>
        private void SumExerciseCounters(PlanDay day)
        {
            // создаем массимы со стартовым значанием "Общее количество"
            day.Counters.WeightLoadsByCategory = new List<ValueEntity>() { new ValueEntity() { Id = -1, Value = day.Counters.WeightLoadSum } };
            day.Counters.LiftCountersByCategory = new List<ValueEntity>() { new ValueEntity() { Id = -1, Value = day.Counters.LiftCounterSum } };
            day.Counters.IntensitiesByCategory = new List<ValueEntity>() { new ValueEntity() { Id = -1, Value = day.Counters.IntensitySum } };

            // группируем данные по категории и записываем значение для каждой из них в массивы
            var groups = day.Exercises.Select(t => t.Exercise).GroupBy(t => t.ExerciseSubTypeId);
            foreach (var item in groups)
            {
                var id = item.First().ExerciseSubTypeId;
                var name = item.First().ExerciseSubTypeName;
                var dayExercises = day.Exercises.Where(t => t.Exercise.ExerciseSubTypeId == id);

                day.Counters.WeightLoadsByCategory.Add(new ValueEntity() { Id = id, Name = name, Value = dayExercises.Sum(t => t.WeightLoad) });
                day.Counters.LiftCountersByCategory.Add(new ValueEntity() { Id = id, Name = name, Value = dayExercises.Sum(t => t.LiftCounter) });
                day.Counters.IntensitiesByCategory.Add(new ValueEntity() { Id = id, Name = name, Value = dayExercises.Sum(t => t.Intensity) / day.Exercises.Count() });
            }
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
