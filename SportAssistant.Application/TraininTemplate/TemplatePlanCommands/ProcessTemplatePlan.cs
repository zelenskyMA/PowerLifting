using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Application.TrainingTemplate.TemplatePlanCommands
{
    public class ProcessTemplatePlan : IProcessTemplatePlan
    {
        private readonly IProcessTemplateDay _processTemplateDay;
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IContextProvider _contextProvider;
        private readonly IUserProvider _user;

        public ProcessTemplatePlan(
            IProcessTemplateDay processTemplateDay,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IContextProvider contextProvider,
            IUserProvider user)
        {
            _processTemplateDay = processTemplateDay;
            _templateSetRepository = templateSetRepository;
            _templatePlanRepository = templatePlanRepository;
            _contextProvider = contextProvider;
            _user = user;
        }

        /// <inheritdoc />
        public async Task DeleteByTemplateSetIdAsync(int id)
        {
            var templatePlansDb = await _templatePlanRepository.FindAsync(t => t.TemplateSetId == id);
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == id && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет прав на удаление сета '{templateSetDb.Name}'");
            }

            foreach (var item in templatePlansDb)
            {
                await DeleteTemplateAsync(item);
            }

            await _contextProvider.AcceptChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteTemplateAsync(TemplatePlanDb entity)
        {
            await _processTemplateDay.DeleteByTemplateIdAsync(entity.Id);
            _templatePlanRepository.Delete(entity);
        }
    }
}
