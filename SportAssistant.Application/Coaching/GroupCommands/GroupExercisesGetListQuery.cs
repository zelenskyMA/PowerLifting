using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Application.Coaching.GroupCommands
{
    /// <summary>
    /// Получение списка групп с назначенными на текущий день подтипами упражнений.
    /// </summary>
    public class GroupExercisesGetListQuery : ICommand<GroupExercisesGetListQuery.Param, List<TrainingGroupExercises>>
    {
        private readonly IProcessGroup _processGroup;
        private readonly IProcessPlanDay _processPlanDay;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;

        public GroupExercisesGetListQuery(
            IProcessGroup processGroup,
            IProcessPlanDay processPlanDay,
            ITrainingGroupUserRepository trainingGroupUserRepository)
        {
            _processGroup = processGroup;
            _processPlanDay = processPlanDay;
            _trainingGroupUserRepository = trainingGroupUserRepository;
        }

        public async Task<List<TrainingGroupExercises>> ExecuteAsync(Param param)
        {
            var results = new List<TrainingGroupExercises>();

            var groups = (await _processGroup.GetGroupsListAsync()).OrderByDescending(t => t.ParticipantsCount).Take(9);

            foreach (var group in groups)
            {
                var userId = (await _trainingGroupUserRepository.GetGroupUsersAsync(group.Id)).FirstOrDefault()?.UserId ?? 0;
                var planDay = userId > 0 ? await _processPlanDay.GetCurrentDay(userId) : null;

                results.Add(new TrainingGroupExercises()
                {
                    Name = group.Name,
                    ParticipantsCount = group.ParticipantsCount,
                    ExerciseTypeCounters = planDay?.ExerciseTypeCounters ?? new List<ValueEntity>(),
                });
            }

            return results.OrderByDescending(t => t.ParticipantsCount).ToList();
        }

        public class Param
        {
        }
    }
}
