using AutoMapper;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingTemplate.TemplateDayCommands;

public class ProcessTemplateDay : IProcessTemplateDay
{
    private readonly IProcessTemplateExercise _processTemplateExercise;
    private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
    private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;
    private readonly ITrainingCountersSetup _trainingCountersSetup;
    private readonly IContextProvider _contextProvider;
    private readonly IMapper _mapper;

    public ProcessTemplateDay(
        IProcessTemplateExercise processTemplateExercise,
        ICrudRepo<TemplateDayDb> templateDayRepository,
        ICrudRepo<TemplateExerciseDb> templateExerciseRepository,
        ITrainingCountersSetup trainingCountersSetup,
        IContextProvider contextProvider,
        IMapper mapper)
    {
        _processTemplateExercise = processTemplateExercise;
        _templateDayRepository = templateDayRepository;
        _templateExerciseRepository = templateExerciseRepository;
        _trainingCountersSetup = trainingCountersSetup;
        _contextProvider = contextProvider;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TemplateDay> GetAsync(int id)
    {
        var dayDb = (await _templateDayRepository.FindAsync(t => t.Id == id)).FirstOrDefault();
        if (dayDb == null)
        {
            return null;
        }

        var templateExercises = await _processTemplateExercise.GetByDaysAsync(new List<int>() { id });
        var percentages = templateExercises.Where(t => t.Settings != null)
            .SelectMany(t => t.Settings.Select(z => z.Percentage))
            .DistinctBy(t => t.Id);

        var day = _mapper.Map<TemplateDay>(dayDb);
        day.Exercises = templateExercises.Where(t => t.TemplateDayId == id).OrderBy(t => t.Order).ToList();
        day.Percentages = templateExercises.Where(t => t.Settings != null)
            .SelectMany(t => t.Settings.Select(z => z.Percentage))
            .DistinctBy(t => t.Id)
            .OrderBy(t => t.MinValue)
            .ToList();

        _trainingCountersSetup.SetDayCounters(day);

        return day;
    }

    /// <inheritdoc />
    public async Task DeleteByTemplateIdAsync(int templateId)
    {
        var templateDaysDb = await _templateDayRepository.FindAsync(t => t.TemplatePlanId == templateId);
        if (templateDaysDb.Count == 0)
        {
            return;
        }

        var dayIds = templateDaysDb.Select(t => t.Id);
        var templateExercisesDb = await _templateExerciseRepository.FindAsync(t => dayIds.Contains(t.TemplateDayId));
        await _processTemplateExercise.DeleteTemplateExercisesAsync(templateExercisesDb);

        _templateDayRepository.DeleteList(templateDaysDb);

        await _contextProvider.AcceptChangesAsync();
    }
}
