using AutoMapper;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

namespace SportAssistant.Application.TrainingTemplate.TemplateSetCommands
{
    /// <summary>
    /// Обновление данных тренировочного шаблона
    /// </summary>
    public class TemplatePlanUpdateCommand : ICommand<TemplatePlanUpdateCommand.Param, int>
    {
        private readonly IProcessTemplateSet _processTemplateSet;
        private readonly IProcessSetUserId _processSetUserId;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IMapper _mapper;

        public TemplatePlanUpdateCommand(
            IProcessTemplateSet processTemplateSet,
            IProcessSetUserId processSetUserId,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            IMapper mapper)
        {
            _processTemplateSet = processTemplateSet;
            _processSetUserId = processSetUserId;
            _templatePlanRepository = templatePlanRepository;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Id == param.TemplatePlan.Id);

            var setId = await VerifyRequestAsync(templatePlanDb, param.TemplatePlan.Name, param.TemplatePlan.Id);

            templatePlanDb = _mapper.Map<TemplatePlanDb>(param.TemplatePlan);
            templatePlanDb.TemplateSetId = setId;

            _templatePlanRepository.Update(templatePlanDb);
            return setId; // нужен для корректной навигации в UI
        }

        private async Task<int> VerifyRequestAsync(TemplatePlanDb templatePlanDb, string newName, int id)
        {
            if (templatePlanDb == null)
            {
                throw new BusinessException($"У вас нет шаблона с ид {id}");
            }

            if (string.IsNullOrEmpty(newName))
            {
                throw new BusinessException("Необходимо указать новое название шаблона");
            }

            var setId = templatePlanDb.TemplateSetId;

            var ownerId = await _processSetUserId.GetBySetId(setId);
            await _processTemplateSet.ChangingAllowedForUserAsync(ownerId);

            var duplicateDb = await _templatePlanRepository.FindOneAsync(t => t.Name == newName && t.TemplateSetId == setId);
            if (duplicateDb != null && duplicateDb.Id != id)
            {
                throw new BusinessException("Тренировочный шаблон с указанным именем уже существует в выбранном цикле");
            }


            return setId;
        }


        public class Param
        {
            public TemplatePlan TemplatePlan { get; set; }
        }
    }
}
