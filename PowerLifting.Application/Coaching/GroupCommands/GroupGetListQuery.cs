using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Получение списка групп тренера по его Ид.
    /// </summary>
    public class GroupGetListQuery : ICommand<GroupGetListQuery.Param, List<TrainingGroup>>
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public GroupGetListQuery(
         ICrudRepo<TrainingGroupDb> trainingGroupRepository,
         ITrainingGroupUserRepository trainingGroupUserRepository,
         IUserProvider user,
         IMapper mapper)
        {
            _trainingGroupRepository = trainingGroupRepository;
            _trainingGroupUserRepository = trainingGroupUserRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<List<TrainingGroup>> ExecuteAsync(Param param)
        {
            var groupsDb = await _trainingGroupRepository.FindAsync(t => t.CoachId == _user.Id);
            if (!groupsDb.Any())
            {
                return new List<TrainingGroup>();
            }

            var groups = groupsDb.Select(t => _mapper.Map<TrainingGroup>(t)).OrderBy(t => t.Name).ToList();
            foreach (var item in groups)
            {
                item.ParticipantsCount = (await _trainingGroupUserRepository.FindAsync(t => t.GroupId == item.Id)).Count();
            }

            return groups.OrderBy(t => t.Name).ToList();
        }

        public class Param { }
    }
}
