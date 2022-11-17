using SportAssistant.Application.Common.Actions;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Application.Coaching.GroupCommands
{
    /// <summary>
    /// Получение списка групп с назначенными на текущий день подтипами упражнений.
    /// </summary>
    public class GroupWorkoutGetListQuery : ICommand<GroupWorkoutGetListQuery.Param, List<TrainingGroupWorkout>>
    {
        private readonly IProcessGroup _processGroup;
        private readonly IProcessPlanDay _processPlanDay;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;

        public GroupWorkoutGetListQuery(
            IProcessGroup processGroup,
            IProcessPlanDay processPlanDay,
            ITrainingGroupUserRepository trainingGroupUserRepository)
        {
            _processGroup = processGroup;
            _processPlanDay = processPlanDay;
            _trainingGroupUserRepository = trainingGroupUserRepository;
        }

        public async Task<List<TrainingGroupWorkout>> ExecuteAsync(Param param)
        {
            var results = new List<TrainingGroupWorkout>();

            var groups = await _processGroup.GetGroupsListAsync();
            foreach (var group in groups)
            {
                var users = await _trainingGroupUserRepository.GetGroupUsersAsync(group.Id);
                foreach (var user in users)
                {
                    var planDay = await _processPlanDay.GetCurrentDay(user.UserId);
                    if (planDay?.Exercises != null && planDay.Exercises.Count > 0)
                    {
                        results.Add(new TrainingGroupWorkout()
                        {
                            Id = user.UserId,
                            Name = UserNaming.GetLegalShortName(user.FirstName, user.Surname, user.Patronimic, "ФИО не указано"),
                            GroupName = group.Name,
                            PlanDay = planDay,
                        });
                    }
                }
            }

            return results.OrderBy(t => t.GroupName).ThenBy(t => t.Name).ToList();
        }

        public class Param
        {
        }
    }
}
