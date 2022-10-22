using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TraininTemplate.TemplateSetCommands
{
    public class TemplateSetGetListQuery : ICommand<TemplateSetGetListQuery.Param, List<TemplateSet>>
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TemplateSetGetListQuery(
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

        public async Task<List<TemplateSet>> ExecuteAsync(Param param)
        {
            var setsDb = await _templateSetRepository.FindAsync(t => t.CoachId == _user.Id);
            var sets = setsDb.Select(t => _mapper.Map<TemplateSet>(t)).OrderBy(t => t.Name).ToList();

            foreach (var item in sets)
            {
                item.Plans = (await _templatePlanRepository.FindAsync(t => t.TemplateSetId == item.Id))
                    .Select(t => _mapper.Map<TemplatePlan>(t)).ToList();
            }

            return sets;
        }

        public class Param { }
    }
}
