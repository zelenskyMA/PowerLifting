using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserAchivementCommands : IUserAchivementCommands
    {
        private readonly IDictionaryCommands _dictionaryCommands;
        private readonly ICrudRepo<UserAchivementDb> _userAchivementRepository;

        private readonly IMapper _mapper;
        private readonly IUserProvider _user;

        public UserAchivementCommands(
            IDictionaryCommands dictionaryCommands,
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
        public async Task<List<UserAchivement>> GetAsync(int userId = 0)
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

        public async Task CreateAsync(List<UserAchivement> achivements)
        {
            var achivementsDb = achivements.Select(t => _mapper.Map<UserAchivementDb>(t));
            foreach (var item in achivementsDb)
            {
                item.CreationDate = DateTime.Now.Date;
                item.UserId = _user.Id;

                var existingAchivement = await _userAchivementRepository.FindAsync(
                    t => t.UserId == item.UserId && t.ExerciseTypeId == item.ExerciseTypeId && t.CreationDate == item.CreationDate);
                if (existingAchivement.Count > 0)
                {
                    existingAchivement.First().Result = item.Result;
                    await _userAchivementRepository.UpdateAsync(existingAchivement.First());
                    continue;
                }

                await _userAchivementRepository.CreateAsync(item);
            }
        }
    }
}
