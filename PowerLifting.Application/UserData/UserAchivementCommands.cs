using AutoMapper;
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

        public UserAchivementCommands(
         ICrudRepo<UserAchivementDb> userAchivementRepository,
         IMapper mapper)
        {
            _userAchivementRepository = userAchivementRepository;
            _mapper = mapper;
        }

        public async Task<List<UserAchivement>> GetAsync(int userId)
        {
            var achivements = await _userAchivementRepository.FindAsync(t => t.UserId == userId);
            if (achivements.Count == 0)
            {
                return new List<UserAchivement>();
            }

            var filteredAchivements = achivements
                .GroupBy(t=> t.ExerciseTypeId)
                .Select(group => group.OrderByDescending(t=> t.CreationDate).First())
                .ToList();

            return filteredAchivements.Select(t => _mapper.Map<UserAchivement>(t)).ToList();
        }

        public async Task<UserAchivement> GetByExerciseTypeAsync(int userId, int exerciseTypeId)
        {
            var achivements = await _userAchivementRepository.FindAsync(t => t.ExerciseTypeId == exerciseTypeId && t.UserId == userId);
            if (achivements.Count == 0)
            {
                return new UserAchivement();
            }

            var achivement = achivements.OrderByDescending(t => t.CreationDate).First();
            return _mapper.Map<UserAchivement>(achivement);
        }
    }
}
