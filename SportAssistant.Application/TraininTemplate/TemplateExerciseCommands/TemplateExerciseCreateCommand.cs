using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands;

/// <summary>
/// Шаблон сета упражнений в рамках тренировочного дня.
/// </summary>
public class TemplateExerciseCreateCommand : ICommand<TemplateExerciseCreateCommand.Param, bool>
{
    private readonly IProcessTemplateExercise _processTemplateExercise;
    private readonly IProcessTemplateSet _processTemplateSet;
    private readonly IProcessSetUserId _processSetUserId;
    private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;

    public TemplateExerciseCreateCommand(
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

    public async Task<bool> ExecuteAsync(Param param)
    {
        if (param.Exercises.Count == 0)
        {
            return false;
        }

        await VerifyRequestAsync(param.DayId);

        //удаляем лишние записи вместе со связями
        var templateExercisesDb = await _templateExerciseRepository.FindAsync(t => t.TemplateDayId == param.DayId);
        if (templateExercisesDb.Count > 0)
        {
            var itemsToDelete = templateExercisesDb.Where(t => !param.Exercises.Select(t => t.PlannedExerciseId).Contains(t.Id)).ToList();
            if (itemsToDelete.Count > 0)
            {
                await _processTemplateExercise.DeleteTemplateExercisesAsync(itemsToDelete);
                foreach (var item in itemsToDelete)
                {
                    templateExercisesDb.Remove(item);
                }
            }
        }

        for (int i = 1; i <= param.Exercises.Count; i++)
        {
            // обновление существующего упражнения
            var templateExercise = templateExercisesDb.FirstOrDefault(t => t.Id == param.Exercises[i - 1].PlannedExerciseId);
            if (templateExercise != null)
            {
                templateExercise.Order = i;
                _templateExerciseRepository.Update(templateExercise);
                continue;
            }

            // добавление нового упражнения
            templateExercise = new TemplateExerciseDb()
            {
                TemplateDayId = param.DayId,
                ExerciseId = param.Exercises[i - 1].Id,
                Order = i
            };

            await _templateExerciseRepository.CreateAsync(templateExercise);
        }
        return true;
    }

    private async Task VerifyRequestAsync(int dayId)
    {
        var ownerId = await _processSetUserId.GetByDayId(dayId);
        await _processTemplateSet.ChangingAllowedForUserAsync(ownerId);
    }

    public class Param
    {
        public int DayId { get; set; }

        public List<Exercise> Exercises { get; set; }
    }
}