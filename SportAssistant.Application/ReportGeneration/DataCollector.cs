using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.ReportGeneration;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.ReportGeneration;
using SportAssistant.Domain.Models.TrainingPlan;
using System.Globalization;

namespace SportAssistant.Application.ReportGeneration;

public class DataCollector : IDataCollector
{
    private readonly IProcessPlanExercise _processPlanExercise;
    private readonly IProcessPlan _processPlan;
    private readonly ICrudRepo<PlanDb> _planRepository;
    private readonly ICrudRepo<PlanDayDb> _planDayRepository;

    public DataCollector(
        IProcessPlanExercise processPlanExercise,
        IProcessPlan processPlan,
        ICrudRepo<PlanDb> planRepository,
        ICrudRepo<PlanDayDb> planDayRepository)
    {
        _processPlanExercise = processPlanExercise;
        _processPlan = processPlan;
        _planRepository = planRepository;
        _planDayRepository = planDayRepository;
    }

    /// <inheritdoc />
    public async Task<ReportData> CollectPlanData(int planId, bool completedOnly)
    {
        var dbPlan = await _planRepository.FindOneAsync(t => t.Id == planId);
        if (dbPlan == null)
        {
            return new ReportData();
        }

        await _processPlan.ViewAllowedForDataOfUserAsync(dbPlan.UserId);

        var planDays = await _planDayRepository.FindAsync(t => t.PlanId == dbPlan.Id);
        var planExercises = await _processPlanExercise.GetByDaysAsync(planDays.Select(t => t.Id).ToList(), completedOnly);

        var report = new ReportData()
        {
            PlanStartDate = dbPlan.StartDate
        };

        // тренировочный день
        foreach (var planDay in planDays.OrderBy(t => t.ActivityDate))
        {
            var planDayExercises = planExercises.Where(t => t.PlanDayId == planDay.Id).ToList();
            if (planDayExercises.Count == 0 || !planDayExercises.Any(t => t.Settings != null && t.Settings.Count > 0))
            {
                continue; // убираем дни без упражнений или с упражнениями, в которых нет поднятий.
            }

            var day = SetReportDay(report, planDay);

            // упражнения
            foreach (var planExercise in planDayExercises.OrderBy(t => t.Order))
            {
                if (planExercise.Settings == null || planExercise.Settings.Count == 0)
                {
                    continue; // убираем упражнения без поднятий
                }

                var exercise = SetReportExercise(day, planExercise);

                exercise.ExerciseSettings = planExercise.Settings.OrderBy(t => t.Weight)
                    .Select(SetReportExerciseSettings)
                    .ToList();
            }
        }

        return report;
    }

    private ReportDay SetReportDay(ReportData report, PlanDayDb planDay)
    {
        var dayDate = planDay.ActivityDate;
        var culture = new CultureInfo("ru-RU", true);

        ReportDay day = new()
        {
            DayDate = $"{culture.TextInfo.ToTitleCase(dayDate.ToString("dddd", culture))} {dayDate.ToString("dd.MM.yyyy")}",
        };

        report.Days.Add(day);
        return day;
    }

    private ReportExercise SetReportExercise(ReportDay reportDay, PlanExercise planExercise)
    {
        ReportExercise exercise = new()
        {
            OrderNumber = planExercise.Order,
            Name = planExercise.Exercise?.Name ?? string.Empty,
            ExtPlanData = planExercise.ExtPlanData,
        };

        reportDay.Exercises.Add(exercise);
        return exercise;
    }

    private ReportExerciseSettings SetReportExerciseSettings(PlanExerciseSettings planSettings)
    {
        return new()
        {
            Weight = planSettings.Weight,
            Iterations = planSettings.Iterations,
            ExercisePart1 = planSettings.ExercisePart1,
            ExercisePart2 = planSettings.ExercisePart2,
            ExercisePart3 = planSettings.ExercisePart3,
        };
    }
}