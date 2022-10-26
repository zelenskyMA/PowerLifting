using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Application.TraininTemplate.TemplateDayCommands
{
    /// <summary>
    /// Получение тренировочного дня из шаблона по Ид.
    /// </summary>
    public class TemplateDayGetByIdQuery : ICommand<TemplateDayGetByIdQuery.Param, TemplateDay>
    {
        private readonly IProcessTemplateDay _processTemplateDay;

        public TemplateDayGetByIdQuery(IProcessTemplateDay processTemplateDay)
        {
            _processTemplateDay = processTemplateDay;
        }

        public async Task<TemplateDay> ExecuteAsync(Param param)
        {
            var templateDay = await _processTemplateDay.GetAsync(param.Id);
            return templateDay;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
