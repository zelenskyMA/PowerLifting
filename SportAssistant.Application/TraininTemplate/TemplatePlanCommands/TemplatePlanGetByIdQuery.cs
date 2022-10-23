using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.TraininTemplate;
using SportAssistant.Infrastructure.Repositories.TrainingPlan;

namespace SportAssistant.Application.TraininTemplate.TemplatePlanCommands
{
    /// <summary>
    /// Получение шаблона для тренировочного плана по Ид.
    /// </summary>
    public class TemplatePlanGetByIdQuery : ICommand<TemplatePlanGetByIdQuery.Param, TemplatePlan>
    {
        private readonly IPlanCountersSetup _planCountersSetup;

        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TemplatePlanGetByIdQuery(
            IPlanCountersSetup planCountersSetup,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateDayDb> templateDayRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _planCountersSetup = planCountersSetup;
            _templateSetRepository = templateSetRepository;
            _templatePlanRepository = templatePlanRepository;
            _templateDayRepository = templateDayRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<TemplatePlan> ExecuteAsync(Param param)
        {
            var templatePlanDb = await _templatePlanRepository.FindOneAsync(t => t.Id == param.Id);
            if (templatePlanDb == null)
            {
                throw new BusinessException("Шаблон не найден");
            }

            var templatePlan = _mapper.Map<TemplatePlan>(templatePlanDb);

            templatePlan.Days = (await _templateDayRepository.FindAsync(t => t.TemplatePlanId == param.Id)).Select(t => _mapper.Map<TemplateDay>(t)).ToList();
            templatePlan.TypeCountersSum = new List<Domain.Models.Common.ValueEntity>();

            return templatePlan;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
