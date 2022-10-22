using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TraininTemplate.TemplateSetCommands
{
    public class TemplateSetGetByIdQuery : ICommand<TemplateSetGetByIdQuery.Param, TemplateSet>
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TemplateSetGetByIdQuery(
            ICrudRepo<TemplateSetDb> templateSetRepository,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _templateSetRepository = templateSetRepository;
            _templatePlanRepository = templatePlanRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<TemplateSet> ExecuteAsync(Param param)
        {
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == param.Id && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет тренировочного цикла с ид {param.Id}");
            }

            var templateSet = _mapper.Map<TemplateSet>(templateSetDb);
            templateSet.Plans = (await _templatePlanRepository.FindAsync(t => t.TemplateSetId == templateSet.Id))
                .Select(t => _mapper.Map<TemplatePlan>(t)).ToList();

            return templateSet;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
