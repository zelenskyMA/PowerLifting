using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands;

/// <summary>
/// Получение запланированного упражнения в шаблоне по его Ид.
/// </summary>
public class TemplateExerciseGetByIdQuery : ICommand<TemplateExerciseGetByIdQuery.Param, TemplateExercise>
{
    private readonly IProcessTemplateExercise _processTemplateExercise;
    private readonly IProcessTemplateSet _processTemplateSet;
    private readonly IProcessSetUserId _processSetUserId;
    private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;

    public TemplateExerciseGetByIdQuery(
        IProcessTemplateExercise processTemplateExercise,
        IProcessTemplateSet processTemplateSet,
        IProcessSetUserId processSetUserId,
        ICrudRepo<TemplateExerciseDb> templateExerciseRepository)
    {
        _processTemplateExercise = processTemplateExercise;
        _processTemplateSet = processTemplateSet;
        _processSetUserId = processSetUserId;
        _templateExerciseRepository = templateExerciseRepository;
    }

    public async Task<TemplateExercise> ExecuteAsync(Param param)
    {
        var templateExerciseDb = await _templateExerciseRepository.FindOneAsync(t => t.Id == param.Id);

        if (templateExerciseDb != null) // запрет просмотра чужих данных
        {
            var ownerId = await _processSetUserId.GetByPlanExerciseId(param.Id);
            await _processTemplateSet.ViewAllowedForDataOfUserAsync(ownerId);
        }
        else
        {
            return new TemplateExercise();
        }

        var request = new List<TemplateExerciseDb>() { templateExerciseDb };
        var exercise = (await _processTemplateExercise.PrepareExerciseDataAsync(request)).FirstOrDefault() ?? new TemplateExercise();
        return exercise;
    }

    public class Param
    {
        public int Id { get; set; }
    }
}
