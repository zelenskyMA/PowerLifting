using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplateDayCommands
{
    /// <summary>
    /// Получение тренировочного дня из шаблона по Ид.
    /// </summary>
    public class TemplateDayGetByIdQuery : ICommand<TemplateDayGetByIdQuery.Param, TemplateDay>
    {
        private readonly IProcessTemplateDay _processTemplateDay;
        private readonly IProcessSetUserId _processSetUserId;
        private readonly IUserProvider _user;

        public TemplateDayGetByIdQuery(
            IProcessTemplateDay processTemplateDay,
            IProcessSetUserId processSetUserId,
            IUserProvider user)
        {
            _processTemplateDay = processTemplateDay;
            _processSetUserId = processSetUserId;
            _user = user;
        }

        public async Task<TemplateDay> ExecuteAsync(Param param)
        {
            var templateDay = await _processTemplateDay.GetAsync(param.Id);

            if (templateDay != null) // чужие данные смотреть нельзя. Сохраняем возможность возвращать  null, когда ничего не нашли.
            {
                var setUserId = await _processSetUserId.GetByDayId(param.Id);
                if (setUserId != _user.Id)
                {
                    throw new DataException();
                }
            }

            return templateDay;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
