using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Analitics;
using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingPlan;
using System.Data;
using static SportAssistant.Domain.Models.Analitics.ChartDataItem;

namespace SportAssistant.Application.Analitics.PlanAnaliticsCommands;

/// <summary>
/// Подготовка и получение аналитики по тренировочным планам выбранного спортсмена
/// </summary>
public class PlanAnaliticsGetQuery : ICommand<PlanAnaliticsGetQuery.Param, PlanAnalitics>
{
    private readonly IProcessPlan _processPlan;
    private readonly IProcessPlanExercise _processPlanExercise;
    private readonly ITrainingCountersSetup _trainingCountersSetup;

    private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
    private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public PlanAnaliticsGetQuery(
        IProcessPlan processPlan,
        IProcessPlanExercise processPlanExercise,
        ITrainingCountersSetup trainingCountersSetup,
        ICrudRepo<PlanDb> trainingPlanRepository,
        ICrudRepo<PlanDayDb> trainingDayRepository,
        IUserProvider user,
        IMapper mapper)
    {
        _processPlan = processPlan;
        _processPlanExercise = processPlanExercise;
        _trainingCountersSetup = trainingCountersSetup;
        _trainingPlanRepository = trainingPlanRepository;
        _trainingDayRepository = trainingDayRepository;
        _user = user;
        _mapper = mapper;
    }

    public async Task<PlanAnalitics> ExecuteAsync(Param param)
    {
        await _processPlan.ViewAllowedForDataOfUserAsync(param.UserId);

        var plans = await PreparePlansWithCounters(param);
        var analitics = new PlanAnalitics();
        if (plans.Count == 0)
        {
            return analitics;
        }

        // "Распрямляем" данные. Перекладываем их из счетной модели в DTO для графика.
        ConvertDataForChart(plans.SelectMany(t => t.Counters.CategoryCountersSum), analitics.CategoryCounters);
        ConvertDataForChart(plans.SelectMany(t => t.Counters.WeightLoadsByCategory), analitics.WeightLoadsByCategory);
        ConvertDataForChart(plans.SelectMany(t => t.Counters.LiftCountersByCategory), analitics.LiftCountersByCategory);
        ConvertDataForChart(plans.SelectMany(t => t.Counters.IntensitiesByCategory), analitics.IntensitiesByCategory);

        // заполняем DTO модель значениями, проходя по всем доступным датам планов
        foreach (var plan in plans.OrderBy(t => t.StartDate).ToList())
        {
            var dataKey = plan.FinishDate.ToString("dd/MM/yy");
            analitics.ChartDotsList.Add(new ChartDot() { Name = dataKey }); // формируем список всех возможных значений для оси Х

            AddValuesToChartDTO(plan.Counters.CategoryCountersSum, analitics.CategoryCounters, dataKey);
            AddValuesToChartDTO(plan.Counters.WeightLoadsByCategory, analitics.WeightLoadsByCategory, dataKey);
            AddValuesToChartDTO(plan.Counters.LiftCountersByCategory, analitics.LiftCountersByCategory, dataKey);
            AddValuesToChartDTO(plan.Counters.IntensitiesByCategory, analitics.IntensitiesByCategory, dataKey);
        }

        return analitics;
    }

    /// <summary>
    /// Собираем данные и считаем метрики по тренировочным дням
    /// </summary>
    /// <param name="param">Период выборки данных</param>
    /// <returns></returns>
    private async Task<List<Plan>> PreparePlansWithCounters(Param param)
    {
        var userId = param.UserId == 0 ? _user.Id : param.UserId;

        // получаем планы, попадающие в выбранный отрезок времени. Для этого уменьшаем начало периода на длину одного плана
        var startPlanDate = param.StartDate.Date.AddDays(-6);
        var finishPlanDate = param.FinishDate.Date;
        var plans = (await _trainingPlanRepository.FindAsync(t => t.UserId == userId &&
            t.StartDate >= startPlanDate && t.StartDate <= finishPlanDate)).Select(_mapper.Map<Plan>).ToList();

        // получаем дни, четко попадающие в указанный период, лишние дни в планах не учитываем
        var planIds = plans.Select(t => t.Id).ToList();
        var planDays = (await _trainingDayRepository.FindAsync(t => planIds.Contains(t.PlanId) &&
            param.StartDate.Date <= t.ActivityDate && t.ActivityDate <= finishPlanDate)).Select(_mapper.Map<PlanDay>).ToList();

        // получаем упражнения и считаем статистику по ним. Записываем в дни
        var planExercises = await _processPlanExercise.GetByDaysAsync(planDays.Select(t => t.Id).ToList(), true); // отсекаем незавершенные поднятия
        foreach (var planDay in planDays)
        {
            planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
            _trainingCountersSetup.SetDayCounters(planDay);
        }

        // считаем статистику по дням и записываем их в планы
        foreach (var plan in plans)
        {
            plan.TrainingDays = planDays.Where(t => t.PlanId == plan.Id).ToList();
            _trainingCountersSetup.SetPlanCounters(plan);
        }

        return plans;
    }
   
    private void ConvertDataForChart(IEnumerable<ValueEntity> sourceList, List<ChartDataItem> targetList)
    {
        foreach (var itemId in sourceList.Select(t => t.Id).Distinct())
        {
            targetList.Add(new ChartDataItem()
            {
                Id = itemId,
                Name = sourceList.FirstOrDefault(t => t.Id == itemId && t.Name != null)?.Name ?? string.Empty,
                Data = new List<KvModel>()
            });
        }
    }

    private void AddValuesToChartDTO(List<ValueEntity> sourceList, List<ChartDataItem> targetList, string dataKey)
    {
        foreach (var item in sourceList)
        {
            var currentTypeCounter = targetList.First(t => t.Id == item.Id);
            currentTypeCounter.Data.Add(new KvModel()
            {
                Name = dataKey,
                Value = item.Value
            });
        }
    }

    public class Param
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }
    }
}
