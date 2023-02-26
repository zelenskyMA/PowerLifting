using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

namespace SportAssistant.Application.TrainingTemplate.TemplatePlanCommands
{
    /// <summary>
    /// Удаление шаблона тренировочного плана
    /// </summary>
    public class TemplatePlanDeleteCommand : ICommand<TemplatePlanDeleteCommand.Param, int>
    {
        private readonly IProcessTemplatePlan _processTemplatePlan;
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IUserProvider _user;

        public TemplatePlanDeleteCommand(
            IProcessTemplatePlan processTemplatePlan,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user)
        {
            _processTemplatePlan = processTemplatePlan;
            _templateSetRepository = templateSetRepository;
            _templatePlanRepository = templatePlanRepository;
            _user = user;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Id == param.Id);
            var setId = await VerifyTemplatePlanAsync(templatePlanDb, param.Id);

            await _processTemplatePlan.DeleteTemplateAsync(templatePlanDb);

            return setId; // нужен для корректной навигации в UI
        }

        private async Task<int> VerifyTemplatePlanAsync(TemplatePlanDb? templatePlanDb, int id)
        {
            
            if (templatePlanDb == null)
            {
                throw new BusinessException($"Шаблон с ид {id} не найден");
            }

            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == templatePlanDb.TemplateSetId && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет прав на удаление шаблона '{templatePlanDb.Name}'");
            }

            return templateSetDb.Id;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
