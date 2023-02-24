using SportAssistant.Application.TrainingTemplate.TemplatePlanCommands;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

namespace SportAssistant.Application.TrainingTemplate.TemplateSetCommands
{
    /// <summary>
    /// Удаление тренировочного цикла
    /// </summary>
    public class TemplateSetDeleteCommand : ICommand<TemplateSetDeleteCommand.Param, bool>
    {
        private readonly IProcessTemplatePlan _processTemplatePlan;
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly IUserProvider _user;

        public TemplateSetDeleteCommand(
            IProcessTemplatePlan processTemplatePlan,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user)
        {
            _processTemplatePlan = processTemplatePlan;
            _templateSetRepository = templateSetRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == param.Id && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет тренировочного цикла с ид {param.Id}");
            }

            await _processTemplatePlan.DeleteByTemplateSetIdAsync(param.Id);
            _templateSetRepository.Delete(templateSetDb);

            return true;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
