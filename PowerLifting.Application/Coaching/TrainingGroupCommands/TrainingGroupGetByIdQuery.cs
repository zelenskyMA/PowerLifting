using AutoMapper;
using PowerLifting.Application.Common;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingGroupCommands
{
    /// <summary>
    /// Получение запланированного упражнения по его Ид.
    /// </summary>
    public class TrainingGroupGetByIdQuery : ICommand<TrainingGroupGetByIdQuery.Param, TrainingGroupInfo>
    {
        private readonly ICrudRepo<TrainingGroupDb> _trainingGroupRepository;
        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
        private readonly IMapper _mapper;

        public TrainingGroupGetByIdQuery(
         ICrudRepo<TrainingGroupDb> trainingGroupRepository,
         ICrudRepo<PlanDb> trainingPlanRepository,
         ITrainingGroupUserRepository trainingGroupUserRepository,
         IMapper mapper)
        {
            _trainingGroupRepository = trainingGroupRepository;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingGroupUserRepository = trainingGroupUserRepository;
            _mapper = mapper;
        }

        public async Task<TrainingGroupInfo> ExecuteAsync(Param param)
        {
            var groupsDb = await _trainingGroupRepository.FindAsync(t => t.Id == param.Id);
            if (!groupsDb.Any())
            {
                throw new BusinessException("Группа не найдена");
            }

            var usersInfoDb = await _trainingGroupUserRepository.GetGroupUsersAsync(param.Id);
            var userIds = usersInfoDb.Select(t => t.UserId).ToList();
            var allActivePlans = await _trainingPlanRepository.FindAsync(
                t => userIds.Contains(t.UserId) && t.StartDate.AddDays(7) >= DateTime.Now.Date);

            var users = usersInfoDb.Select(t => new GroupUser()
            {
                Id = t.UserId,
                FullName = Naming.GetLegalFullName(t.FirstName, t.Surname, t.Patronimic),
                ActivePlansCount = allActivePlans.Count(t => t.UserId == t.UserId),
            }).OrderBy(t => t.FullName).ToList();

            var groupInfo = new TrainingGroupInfo()
            {
                Group = _mapper.Map<TrainingGroup>(groupsDb.First()),
                Users = users
            };

            return groupInfo;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
