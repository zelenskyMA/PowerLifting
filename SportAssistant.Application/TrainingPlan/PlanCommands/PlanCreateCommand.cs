﻿using SportAssistant.Domain;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingPlan.PlanCommands;

/// <summary>
/// Создание нового тренировочного плана на неделю от указанной даты.
/// </summary>
public class PlanCreateCommand : ICommand<PlanCreateCommand.Param, int>
{
    private readonly IProcessPlan _processPlan;
    private readonly IProcessPlanDay _processPlanDay;
    private readonly ICrudRepo<PlanDb> _planRepository;
    private readonly IContextProvider _provider;

    public PlanCreateCommand(
        IProcessPlan processPlan,
        IProcessPlanDay processPlanDay,
        ICrudRepo<PlanDb> planRepository,
        IContextProvider provider)
    {
        _processPlan = processPlan;
        _planRepository = planRepository;
        _processPlanDay = processPlanDay;
        _provider = provider;
    }

    public async Task<int> ExecuteAsync(Param param)
    {
        param.CreationDate = param.CreationDate.Date;

        if (param.DaysCount > 7 || param.DaysCount == 0)
        {
            throw new BusinessException("Можно включить от 1 до 7 дней в один тренировочный план.");
        }

        var userId = await _processPlan.PlanningAllowedForUserAsync(param.UserId);
        await _processPlan.CheckActivePlansLimitAsync(userId);

        var crossingPlansDb = await _processPlan.GetCrossingPlansAsync(param.CreationDate, userId, param.DaysCount);
        if (crossingPlansDb.Any())
        {
            string errorDates = string.Join(", ", crossingPlansDb.Select(t => t.StartDate.ToString("dd/MM/yyyy")));
            throw new BusinessException($"Найдены пересекающийся по датам планы. Даты начала: {errorDates}");
        }

        var plan = new PlanDb() { StartDate = param.CreationDate, UserId = userId };
        await _planRepository.CreateAsync(plan);
        await _provider.AcceptChangesAsync();

        for (int i = 0; i < param.DaysCount; i++)
        {
            await _processPlanDay.CreateAsync(userId, plan.Id, param.CreationDate.AddDays(i));
        }

        return plan.Id;
    }

    public class Param
    {
        public DateTime CreationDate { get; set; }

        public int DaysCount { get; set; } = AppConstants.DaysInPlan;

        public int UserId { get; set; } = 0;
    }
}
