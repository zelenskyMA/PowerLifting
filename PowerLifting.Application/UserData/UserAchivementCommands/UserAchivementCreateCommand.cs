using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData.UserCommands1
{
    public class UserAchivementCreateCommand : ICommand<UserAchivementCreateCommand.Param, bool>
    {
        private readonly ICrudRepo<UserAchivementDb> _userAchivementRepository;
        private readonly IMapper _mapper;
        private readonly IUserProvider _user;

        public UserAchivementCreateCommand(
            ICrudRepo<UserAchivementDb> userAchivementRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _userAchivementRepository = userAchivementRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            var achivementsDb = param.Achivements.Select(t => _mapper.Map<UserAchivementDb>(t));
            foreach (var item in achivementsDb)
            {
                item.CreationDate = DateTime.Now.Date;
                item.UserId = _user.Id;

                var existingAchivement = await _userAchivementRepository.FindAsync(
                    t => t.UserId == item.UserId && t.ExerciseTypeId == item.ExerciseTypeId && t.CreationDate == item.CreationDate);
                if (existingAchivement.Count > 0)
                {
                    existingAchivement.First().Result = item.Result;
                    _userAchivementRepository.Update(existingAchivement.First());
                    continue;
                }

                await _userAchivementRepository.CreateAsync(item);
            }

            return true;
        }

        public class Param
        {
            public List<UserAchivement> Achivements { get; set; }
        }
    }
}
