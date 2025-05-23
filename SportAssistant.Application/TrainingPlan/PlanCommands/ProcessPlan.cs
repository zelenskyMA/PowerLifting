﻿using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanCommands;

public class ProcessPlan : IProcessPlan
{
    private readonly IProcessUserInfo _processUserInfo;
    private readonly IProcessPlanDay _processPlanDay;
    private readonly IProcessSettings _processSettings;
    private readonly ICrudRepo<PlanDb> _planRepository;
    private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
    private readonly ICrudRepo<PlanDayDb> _planDayRepository;
    private readonly IContextProvider _provider;
    private readonly IUserProvider _user;

    public ProcessPlan(
        IProcessUserInfo processUserInfo,
        IProcessPlanDay processPlanDay,
        IProcessSettings processSettings,
        ICrudRepo<PlanDb> planRepository,
        ICrudRepo<TemplateDayDb> templateDayRepository,
        ICrudRepo<PlanDayDb> planDayRepository,
        IContextProvider provider,
        IUserProvider user)
    {
        _processUserInfo = processUserInfo;
        _processPlanDay = processPlanDay;
        _processSettings = processSettings;
        _planRepository = planRepository;
        _templateDayRepository = templateDayRepository;
        _planDayRepository = planDayRepository;
        _provider = provider;
        _user = user;
    }

    /// <inheritdoc />
    public async Task<int> AssignPlanAsync(int templateId, DateTime creationDate, int userId)
    {
        await CheckActivePlansLimitAsync(userId);

        var crossingPlansDb = await GetCrossingPlansAsync(creationDate, userId);
        foreach (var planDb in crossingPlansDb)
        {
            await _processPlanDay.DeleteDayByPlanIdAsync(planDb.Id);
            _planRepository.Delete(planDb);
            await _provider.AcceptChangesAsync();
        }

        var plan = new PlanDb() { StartDate = creationDate, UserId = userId };
        await _planRepository.CreateAsync(plan);
        await _provider.AcceptChangesAsync();

        var templateDays = (await _templateDayRepository.FindAsync(t => t.TemplatePlanId == templateId)).OrderBy(t => t.DayNumber).ToList();
        for (int i = 0; i < AppConstants.DaysInPlan; i++)
        {
            var dayId = await _processPlanDay.CreateAsync(userId, plan.Id, creationDate.AddDays(i), templateDays[i].Id);
        }

        return plan.Id;
    }

    /// <inheritdoc />
    public async Task<int> PlanningAllowedForUserAsync(int userIdForCheck)
    {
        if (userIdForCheck == 0 || userIdForCheck == _user.Id) // план для себя
        {
            return _user.Id;
        }

        var info = await _processUserInfo.GetInfo(userIdForCheck);
        if (info?.CoachId != _user.Id) // тренерский план спортсмену
        {
            throw new BusinessException("У вас нет права планировать тренировки данного пользователя");
        }
        return userIdForCheck;
    }

    /// <inheritdoc />
    public async Task<bool> ViewAllowedForDataOfUserAsync(int userIdForCheck)
    {
        if (userIdForCheck == 0 || userIdForCheck == _user.Id) // проверяем на свой запрос
        {
            return true;
        }

        var info = await _processUserInfo.GetInfo(userIdForCheck);
        if (info?.CoachId != _user.Id) // проверяем на тренерский запрос
        {
            throw new DataException(); // не свой и не тренерский
        }

        return true;
    }

    /// <inheritdoc />
    public async Task CheckActivePlansLimitAsync(int userId)
    {
        var startDate = DateTime.Now.Date.AddDays(AppConstants.DaysInPlan);
        var activePlans = await _planRepository.FindAsync(t => t.UserId == userId && t.StartDate > startDate);

        var settings = await _processSettings.GetAsync();
        if (activePlans.Count() > settings.MaxActivePlans)
        {
            throw new BusinessException($"Уже запланировано больше {settings.MaxActivePlans} недель. Дождитесь выполнения хотя бы одного плана.");
        }
    }

    /// <inheritdoc />
    public async Task<List<PlanDb>> GetCrossingPlansAsync(DateTime creationDate, int userId, int daysCount = 0)
    {
        daysCount = daysCount == 0 ? AppConstants.DaysInPlan : daysCount;

        var prevPlanDate = creationDate.AddDays(-(daysCount - 1));
        var nextPlanDate = creationDate.AddDays(daysCount - 1);

        var crossingPlans = await _planRepository.FindAsync(t =>
            t.UserId == userId &&
            t.StartDate >= prevPlanDate &&
        t.StartDate <= nextPlanDate);

        //не все планы на 7 дней. Берем по максимуму и убираем те, которые не дотягивают последним днем до начала нового плана.
        foreach (var plan in crossingPlans.ToArray())
        {
            var planDay = (await _planDayRepository.FindAsync(t => t.PlanId == plan.Id)).OrderByDescending(t => t.ActivityDate).First();
            if (planDay.ActivityDate < creationDate)
            {
                crossingPlans.Remove(plan);
            }
        }

        return crossingPlans;
    }
}
