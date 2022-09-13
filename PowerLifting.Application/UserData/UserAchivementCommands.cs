using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserAchivementCommands : IUserAchivementCommands
    {
        private readonly ICrudRepo<UserAchivementDb> _userAchivementRepository;
        private readonly IMapper _mapper;
        private readonly IUserProvider _user;

        public UserAchivementCommands(
            IUserProvider user,
            ICrudRepo<UserAchivementDb> userAchivementRepository,
            IMapper mapper)
        {
            _userAchivementRepository = userAchivementRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<UserAchivement>> GetAsync()
        {
            var achivements = await _userAchivementRepository.FindAsync(t => t.UserId == _user.Id);
            if (achivements.Count == 0)
            {
                return new List<UserAchivement>();
            }

            var filteredAchivements = achivements
                .GroupBy(t => t.ExerciseTypeId)
                .Select(group => group.OrderByDescending(t => t.CreationDate).First())
                .ToList();

            return filteredAchivements.Select(t => _mapper.Map<UserAchivement>(t)).ToList();
        }

        /// <inheritdoc />
        public async Task<UserAchivement> GetByExerciseTypeAsync(int exerciseTypeId)
        {
            var achivements = await _userAchivementRepository.FindAsync(t => t.ExerciseTypeId == exerciseTypeId && t.UserId == _user.Id);
            if (achivements.Count == 0)
            {
                return new UserAchivement();
            }

            var achivement = achivements.OrderByDescending(t => t.CreationDate).First();
            return _mapper.Map<UserAchivement>(achivement);
        }
    }
}
