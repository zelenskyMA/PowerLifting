using AutoMapper;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplatePlanCommands;

/// <summary>
/// Получение шаблона для тренировочного плана по Ид.
/// </summary>
public class TemplatePlanGetByIdQuery : ICommand<TemplatePlanGetByIdQuery.Param, TemplatePlan>
{
    private readonly ITrainingCountersSetup _trainingCountersSetup;
    private readonly IProcessTemplateExercise _processTemplateExercise;
    private readonly IProcessTemplateSet _processTemplateSet;
    private readonly IProcessSetUserId _processSetUserId;

    private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
    private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
    private readonly IMapper _mapper;

    public TemplatePlanGetByIdQuery(
        ITrainingCountersSetup trainingCountersSetup,
        IProcessTemplateExercise processTemplateExercise,
        IProcessTemplateSet processTemplateSet,
        IProcessSetUserId processSetUserId,
        ICrudRepo<TemplatePlanDb> templatePlanRepository,
        ICrudRepo<TemplateDayDb> templateDayRepository,
        IMapper mapper)
    {
        _trainingCountersSetup = trainingCountersSetup;
        _processTemplateExercise = processTemplateExercise;
        _processTemplateSet = processTemplateSet;
        _processSetUserId = processSetUserId;
        _templatePlanRepository = templatePlanRepository;
        _templateDayRepository = templateDayRepository;
        _mapper = mapper;
    }

    public async Task<TemplatePlan> ExecuteAsync(Param param)
    {
        var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Id == param.Id);
        if (templatePlanDb == null)
        {
            throw new BusinessException("Шаблон не найден");
        }

        if (templatePlanDb != null) // чужие данные смотреть нельзя
        {
            var ownerId = await _processSetUserId.GetByPlanId(param.Id);
            await _processTemplateSet.ViewAllowedForDataOfUserAsync(ownerId);
        }

        var templatePlan = _mapper.Map<TemplatePlan>(templatePlanDb);

        var days = (await _templateDayRepository.FindAsync(t => t.TemplatePlanId == templatePlanDb.Id)).Select(_mapper.Map<TemplateDay>).ToList();
        var exercises = await _processTemplateExercise.GetByDaysAsync(days.Select(t => t.Id).ToList());
        foreach (var day in days)
        {
            day.Exercises = exercises.Where(t => t.TemplateDayId == day.Id).OrderBy(t => t.Order).ToList();
            _trainingCountersSetup.SetDayCounters(day);
        }

        templatePlan.TrainingDays = days;
        _trainingCountersSetup.SetPlanCounters(templatePlan);

        return templatePlan;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
