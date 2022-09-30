using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching
{
    public class TrainingGroupCommands : ITrainingGroupCommands
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
        private readonly ICrudRepo<UserTrainingGroupDb> _userTrainingGroupRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TrainingGroupCommands(
         ICrudRepo<TrainingGroupDb> trainingGroupRepository,
         ICrudRepo<UserTrainingGroupDb> userTrainingGroupRepository,
         IUserProvider user,
         IMapper mapper)
        {
            _trainingGroupRepository = trainingGroupRepository;
            _userTrainingGroupRepository = userTrainingGroupRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<TrainingGroup>> GetListAsync()
        {
            var groupsDb = await _trainingGroupRepository.FindAsync(t => t.CoachId == _user.Id);
            if (!groupsDb.Any())
            {
                return new List<TrainingGroup>();
            }

            var groups = groupsDb.Select(t => _mapper.Map<TrainingGroup>(t)).OrderBy(t => t.Name).ToList();
            return groups;
        }

        /// <inheritdoc />
        public async Task CreateAsync(TrainingGroup group)
        {
            var groupDb = await _trainingGroupRepository.FindAsync(t => t.Name == group.Name && t.CoachId == _user.Id);
            if (groupDb.Any())
            {
                throw new BusinessException($"Группа с названием '{group.Name}' уже существует");
            }

            await _trainingGroupRepository.CreateAsync(new TrainingGroupDb()
            {
                Name = group.Name,
                Description = group.Description,
                CoachId = _user.Id
            });
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TrainingGroup group)
        {
            var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == group.Id);
            if (!groupsDb.Any())
            {
                throw new BusinessException($"Группа не существует");
            }

            var groupDb = groupsDb.First();
            if (groupDb.Name != group.Name)
            {
                var duplicateGroupsDb = await _trainingGroupRepository.FindAsync(t => t.Name == group.Name);
                if (groupsDb.Count() > 0)
                {
                    throw new BusinessException($"Группа с названием '{group.Name}' уже существует. Измените название на другое.");
                }
            }

            groupDb.Description = group.Description;
            groupDb.Name = group.Name;

            await _trainingGroupRepository.UpdateAsync(groupDb);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var groupDb = await _trainingGroupRepository.FindAsync(t => t.Id == id);
            if (!groupDb.Any())
            {
                throw new BusinessException($"Группа с Id '{id}' не найдена");
            }

            var groupUsersDb = await _userTrainingGroupRepository.FindAsync(t => t.GroupId == id);
            if (groupUsersDb.Any())
            {
                throw new BusinessException($"В удаляемой группе устались участники");
            }

            await _trainingGroupRepository.DeleteAsync(groupDb.First());
        }
    }
}
