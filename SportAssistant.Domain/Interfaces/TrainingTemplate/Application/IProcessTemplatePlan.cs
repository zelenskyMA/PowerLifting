using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;

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
    }
}