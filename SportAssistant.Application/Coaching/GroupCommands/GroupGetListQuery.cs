using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Получение списка групп тренера по его Ид.
    /// </summary>
    public class GroupGetListQuery : ICommand<GroupGetListQuery.Param, List<TrainingGroup>>
    {
        private readonly IProcessGroup _processGroup;

        public GroupGetListQuery(
         IProcessGroup processGroup)
        {
            _processGroup = processGroup;
        }

        public async Task<List<TrainingGroup>> ExecuteAsync(Param param)
        {
            var groups = await _processGroup.GetGroupsListAsync();
            return groups.OrderBy(t => t.Name).ToList();
        }

        public class Param { }
    }
}
