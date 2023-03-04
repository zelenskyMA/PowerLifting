using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

namespace SportAssistant.Application.TrainingTemplate.TemplateSetCommands
{
    /// <summary>
    /// Обновление данных тренировочного цикла
    /// </summary>
    public class TemplateSetUpdateCommand : ICommand<TemplateSetUpdateCommand.Param, bool>
    {
        private readonly IProcessTemplatePlan _processTemplatePlan;
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TemplateSetUpdateCommand(
            IProcessTemplatePlan processTemplatePlan,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _processTemplatePlan = processTemplatePlan;
            _templateSetRepository = templateSetRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == param.TemplateSet.Id && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет тренировочного цикла с ид {param.TemplateSet.Id}");
            }

            if (templateSetDb.Name != param.TemplateSet.Name)
            {
                await UpdateSetNameAsync(param, templateSetDb);
            }

            await _processTemplatePlan.UpdateTemplatesInSet(param.TemplateSet.Templates);

            return true;
        }

        private async Task UpdateSetNameAsync(Param param, TemplateSetDb templateSetDb) {
            if (string.IsNullOrEmpty(param.TemplateSet.Name))
            {
                throw new BusinessException("Необходимо указать новое название цикла");
            }

            var duplicateSetDb = await _templateSetRepository.FindOneAsync(t => t.Name == param.TemplateSet.Name && t.CoachId == _user.Id);
            if (duplicateSetDb != null && duplicateSetDb.Id != param.TemplateSet.Id)
            {
                throw new BusinessException("Тренировочный цикл с указанным именем уже существует");
            }

            templateSetDb = _mapper.Map<TemplateSetDb>(param.TemplateSet);
            templateSetDb.CoachId = _user.Id;

            _templateSetRepository.Update(templateSetDb);
        }

        public class Param
        {
            public TemplateSet TemplateSet { get; set; }
        }
    }
}
