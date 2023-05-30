using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

public interface IProcessTemplateExercise
{
    /// <summary>
    /// Получение запланированных упражнений по Ид дней в плане.
    /// </summary>
    /// <param name="dayIds">Список Ид дней в планах</param>
    /// <returns></returns>
    Task<List<TemplateExercise>> GetByDaysAsync(List<int> dayIds);

    /// <summary>
    /// Обогащаем данными модель запланированного упражнения
    /// </summary>
    /// <param name="templateExerciseDb">Данные по упражнению из БД</param>
    /// <returns></returns>
    Task<List<TemplateExercise>> PrepareExerciseDataAsync(List<TemplateExerciseDb> templateExerciseDb);

    /// <summary>
    /// Удаление запланированных упражнений вместе с поднятиями (exerciseSettings)
    /// </summary>
    /// <param name="templateExercises">Список для удаления</param>
    /// <returns></returns>
    Task DeleteTemplateExercisesAsync(List<TemplateExerciseDb> templateExercises);
}
