using AutoMapper;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserInfoCommands
{
    public class ProcessUserInfo : IProcessUserInfo
    {
        private readonly IUserRoleCommands _userRoleCommands;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IMapper _mapper;

        public ProcessUserInfo(
            IUserRoleCommands userRoleCommands,
            ICrudRepo<UserInfoDb> userInfoRepository,
            IMapper mapper)
        {
            _userRoleCommands = userRoleCommands;
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<UserInfo> GetInfo(int userId)
        {
            var infoDb = (await _userInfoRepository.FindAsync(t => t.UserId == userId)).FirstOrDefault();
            if (infoDb == null)
            {
                return new UserInfo();
            }

            var info = _mapper.Map<UserInfo>(infoDb);
            info.LegalName = UserNaming.GetLegalShortName(info.FirstName, info.Surname, info.Patronimic, "Кабинет");
            info.RolesInfo = await _userRoleCommands.GetUserRoles(userId);

            if (info.CoachId > 0)
            {
                info.CoachLegalName = (await GetInfo(info.CoachId.Value)).LegalName;
            }

            return info;
        }
    }
}
