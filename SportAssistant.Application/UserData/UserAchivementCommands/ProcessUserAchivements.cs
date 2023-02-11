using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserAchivementCommands
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

            return filteredAchivements.Select(_mapper.Map<UserAchivement>).ToList();
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
