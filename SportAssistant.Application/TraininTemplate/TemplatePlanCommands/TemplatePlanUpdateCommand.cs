using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TrainingTemplate.TemplateSetCommands
{
    /// <summary>
    /// Обновление данных тренировочного шаблона
    /// </summary>
    public class TemplatePlanUpdateCommand : ICommand<TemplatePlanUpdateCommand.Param, int>
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TemplatePlanUpdateCommand(
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _templateSetRepository = templateSetRepository;
            _templatePlanRepository = templatePlanRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Id == param.TemplatePlan.Id);
            if (templatePlanDb == null)
            {
                throw new BusinessException($"У вас нет шаблона с ид {param.TemplatePlan.Id}");
            }

            var setId = templatePlanDb.TemplateSetId;
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == setId && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет прав на удаление выбранного шаблона");
            }


            templatePlanDb = _mapper.Map<TemplatePlanDb>(param.TemplatePlan);
            templatePlanDb.TemplateSetId = setId;

            _templatePlanRepository.Update(templatePlanDb);
            return setId;
        }

        public class Param
        {
            public TemplatePlan TemplatePlan { get; set; }
        }
    }
}
