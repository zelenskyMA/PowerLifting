using AutoMapper;
using PowerLifting.Application.Common;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData.UserInfoCommands
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
            info.LegalName = Naming.GetLegalShortName(info.FirstName, info.Surname, info.Patronimic, "Кабинет");
            info.RolesInfo = await _userRoleCommands.GetUserRoles(userId);

            if (info.CoachId > 0)
            {
                info.CoachLegalName = (await GetInfo(info.CoachId.Value)).LegalName;
            }

            return info;
        }
    }
}
