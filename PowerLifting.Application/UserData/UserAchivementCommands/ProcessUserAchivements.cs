using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData.UserAchivementCommands
{
    public class ProcessUserAchivements : IProcessUserAchivements
    {
        private readonly IProcessDictionary _dictionaryCommands;
        private readonly ICrudRepo<UserAchivementDb> _userAchivementRepository;

        private readonly IMapper _mapper;
        private readonly IUserProvider _user;

        public ProcessUserAchivements(
            IProcessDictionary dictionaryCommands,
            ICrudRepo<UserAchivementDb> userAchivementRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _dictionaryCommands = dictionaryCommands;
            _userAchivementRepository = userAchivementRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<UserAchivement>> GetAsync(int userId)
        {
            userId = userId == 0 ? _user.Id : userId;
            var achivements = await _userAchivementRepository.FindAsync(t => t.UserId == userId);
            if (achivements.Count == 0)
            {
                var dictionaryAchivements = await _dictionaryCommands.GetItemsByTypeIdAsync(DictionaryTypes.ExerciseType);
                foreach (var item in dictionaryAchivements)
                {
                    achivements.Add(new UserAchivementDb()
                    {
                        UserId = userId,
                        Result = 0,
                        ExerciseTypeId = item.Id,
                        CreationDate = DateTime.Now
                    });
                }
            }

            var filteredAchivements = achivements
                .GroupBy(t => t.ExerciseTypeId)
                .Select(group => group.OrderByDescending(t => t.CreationDate).First())
                .ToList();

            return filteredAchivements.Select(t => _mapper.Map<UserAchivement>(t)).ToList();
        }

        /// <inheritdoc />
        public async Task<UserAchivement> GetByExerciseTypeAsync(int userId, int exerciseTypeId)
        {
            userId = userId == 0 ? _user.Id : userId;
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
