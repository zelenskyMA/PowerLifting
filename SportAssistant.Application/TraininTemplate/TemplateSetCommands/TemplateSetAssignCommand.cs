using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.TraininTemplate.TemplateSetCommands
{
    /// <summary>
    /// Назначение тренировочного цикла, или шаблона в цикле, группе спортсменов 
    /// </summary>
    public class TemplateSetAssignCommand : ICommand<TemplateSetAssignCommand.Param, bool>
    {
        private readonly IProcessPlan _processPlan;
        private readonly IProcessGroup _processGroup;
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly IUserProvider _user;

        public TemplateSetAssignCommand(
            IProcessPlan processPlan,
            IProcessGroup processGroup,
            ICrudRepo<TemplateSetDb> templateSetRepository,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            IUserProvider user)
        {
            _processPlan = processPlan;
            _processGroup = processGroup;
            _templatePlanRepository = templatePlanRepository;
            _templateSetRepository = templateSetRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            (List<TemplatePlanDb> templates, TrainingGroupInfo groupInfo) = await ValidateAssignmentAsync(param);

            foreach (var user in groupInfo.Users)
            {
                try
                {
                    var creationDate = param.StartDate;
                    foreach (var item in templates)
                    {
                        var planId = await _processPlan.AssignPlanAsync(item.Id, creationDate, user.Id);
                        creationDate = creationDate.AddDays(6);
                    }
                }
                catch (BusinessException ex)
                {

                    throw new BusinessException($"Назначение спортсмену '{user.FullName}' не удалось. {ex.Message}");
                }
            }

            return true;
        }

        private async Task<(List<TemplatePlanDb> templates, TrainingGroupInfo groupInfo)> ValidateAssignmentAsync(Param param)
        {
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == param.SetId && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет тренировочного цикла с ид {param.SetId}");
            }

            var templates = await _templatePlanRepository.FindAsync(t => t.TemplateSetId == templateSetDb.Id);
            if (param.TemplateId != 0)
            {
                templates = templates.Where(t => t.Id == param.TemplateId).ToList();
            }

            var groupInfo = await _processGroup.GetGroupInfoByIdAsync(param.GroupId);
            if (groupInfo.Group.CoachId != _user.Id)
            {
                throw new BusinessException($"У вас нет прав назначать тренировки группе '{groupInfo.Group.Name}'");
            }

            return (templates, groupInfo);
        }


        public class Param
        {
            public int SetId { get; set; }

            public int TemplateId { get; set; }

            public int GroupId { get; set; }

            public DateTime StartDate { get; set; }
        }
    }
}
