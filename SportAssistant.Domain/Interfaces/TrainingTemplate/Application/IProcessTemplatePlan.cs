using SportAssistant.Domain.DbModels.TrainingTemplate;

namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application
{
    public interface IProcessTemplatePlan
    {
        /// <summary>
        /// Удаление шаблонов по Ид цикла
        /// </summary>
        /// <param name="id">Ид тренировочного цикла</param>
        /// <returns></returns>
        Task DeleteByTemplateSetIdAsync(int id);

        /// <summary>
        /// Удаление шаблона тренировочного плана
        /// </summary>
        /// <param name="entity">Шаблон</param>
        /// <returns></returns>
        Task DeleteTemplateAsync(TemplatePlanDb entity);

        /// <summary>
        /// Обновляем порядок шаблонов в цикле
        /// </summary>
        /// <param name="templatesList">список шаблонов из цикла</param>
        /// <returns></returns>
        Task UpdateTemplatesInSet(List<TemplatePlan> templatesList);
    }
}