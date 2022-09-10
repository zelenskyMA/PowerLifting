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


        public async Task<UserAchivement> GetByExerciseType(int userId, int exerciseTypeId)
        {
            var achivement = (await _userAchivementRepository.FindAsync(
                t => t.ExerciseTypeId == exerciseTypeId && t.UserId == userId)).FirstOrDefault();

            if (achivement == null)
            {
                return new UserAchivement();
            }

            return _mapper.Map<UserAchivement>(achivement);
        }
    }
}
