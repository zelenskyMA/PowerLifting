using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application
{
    public interface IProcessTemplateExerciseSettings
    {

        /// <summary>
        /// Получение поднятий в упражнениях по списку Ид упражнений
        /// </summary>
        /// <param name="exerciseIds">Список Ид запланированных упражнений</param>
        /// <returns></returns>
        Task<List<TemplateExerciseSettings>> GetAsync(List<int> exerciseIds);       

        /// <summary>
        /// Удаление поднятий в упражнениях из списка
        /// </summary>
        /// <param name="exerciseIds">Поднятия для удаления</param>
        /// <returns></returns>
        Task DeleteByTemplateExerciseIdsAsync(List<int> exerciseIds);

        /// <summary>
        /// Обновление поднятий по списку в упражнении
        /// </summary>
        /// <param name="templateExerciseId">Ид упражнения в шаблоне</param>
        /// <param name="settingsList">Список поднятий</param>
        /// <returns></returns>
        Task UpdateAsync(int templateExerciseId, List<TemplateExerciseSettings> settingsList);
    }
}
