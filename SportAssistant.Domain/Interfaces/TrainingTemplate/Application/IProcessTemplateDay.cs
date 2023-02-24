using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application
{
    public interface IProcessTemplateDay
    {
        /// <summary>
        /// Получение дня в шаблоне по Ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TemplateDay> GetAsync(int id);

        /// <summary>
        /// Удаление дней из шаблона по его идентификатору.
        /// </summary>
        /// <param name="templateId">Ид шаблона</param>
        /// <returns></returns>
        Task DeleteByTemplateIdAsync(int templateId);
    }
}
