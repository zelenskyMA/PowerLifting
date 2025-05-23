﻿using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.PlanCommands;

/// <summary>
/// Получение тренировочного плана по Ид.
/// </summary>
public class PlanGetByIdQuery : ICommand<PlanGetByIdQuery.Param, Plan>
{
    private readonly IProcessPlanExercise _processPlanExercise;
    private readonly ITrainingCountersSetup _planCountersSetup;
    private readonly IProcessPlan _processPlan;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<PlanDb> _planRepository;
    private readonly ICrudRepo<PlanDayDb> _planDayRepository;
    private readonly IUserProvider _user;
    private readonly IMapper _mapper;

    public PlanGetByIdQuery(
        IProcessPlanExercise processPlanExercise,
        ITrainingCountersSetup planCountersSetup,
        IProcessPlan processPlan,
        IProcessUserInfo processUserInfo,
        ICrudRepo<PlanDb> planRepository,
        ICrudRepo<PlanDayDb> planDayRepository,
        IUserProvider user,
        IMapper mapper)
    {
        _processPlanExercise = processPlanExercise;
        _planCountersSetup = planCountersSetup;
        _processPlan = processPlan;
        _processUserInfo = processUserInfo;
        _planRepository = planRepository;
        _planDayRepository = planDayRepository;
        _user = user;
        _mapper = mapper;
    }

    public async Task<Plan> ExecuteAsync(Param param)
    {
        var dbPlan = await _planRepository.FindOneAsync(t => t.Id == param.Id);
        if (dbPlan == null)
        {
            return new Plan();
        }

        await _processPlan.ViewAllowedForDataOfUserAsync(dbPlan.UserId);

        var plan = _mapper.Map<Plan>(dbPlan);

        var planDays = (await _planDayRepository.FindAsync(t => t.PlanId == dbPlan.Id)).Select(_mapper.Map<PlanDay>).ToList();
        var planExercises = await _processPlanExercise.GetByDaysAsync(planDays.Select(t => t.Id).ToList());
        foreach (var planDay in planDays)
        {
            planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
            _planCountersSetup.SetDayCounters(planDay);
        }

        plan.TrainingDays = planDays;
        plan.IsMyPlan = plan.UserId == _user.Id;
        _planCountersSetup.SetPlanCounters(plan);

        if (!plan.IsMyPlan)
        {
            var info = await _processUserInfo.GetInfo(plan.UserId);
            plan.Owner.Name = info.LegalName;
        }

        return plan;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
