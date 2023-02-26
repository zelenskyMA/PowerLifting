using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Получение тренерской группы по ее Ид.
    /// </summary>
    public class GroupGetByIdQuery : ICommand<GroupGetByIdQuery.Param, TrainingGroupInfo>
    {
        private readonly IProcessGroup _processGroup;
        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;

        public GroupGetByIdQuery(
            IProcessGroup processGroup,
            ICrudRepo<PlanDb> trainingPlanRepository)
        {
            _processGroup = processGroup;
            _trainingPlanRepository = trainingPlanRepository;
        }

        public async Task<TrainingGroupInfo> ExecuteAsync(Param param)
        {
            var groupInfo = await _processGroup.GetGroupInfoByIdAsync(param.Id);

            var userIds = groupInfo.Users.Select(t => t.Id).ToList();
            var allActivePlans = await _trainingPlanRepository.FindAsync(t =>
                userIds.Contains(t.UserId) &&
                t.StartDate.AddDays(7) >= DateTime.Now.Date);

            foreach (var user in groupInfo.Users)
            {
                user.ActivePlansCount = allActivePlans.Count(t => t.UserId == user.Id);
            }

            return groupInfo;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
