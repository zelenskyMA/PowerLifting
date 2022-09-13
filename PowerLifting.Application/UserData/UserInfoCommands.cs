using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData
{
    public class UserInfoCommands : IUserInfoCommands
    {
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserInfoCommands(
            ICrudRepo<UserInfoDb> userInfoRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _userInfoRepository = userInfoRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<UserInfo> GetAsync()
        {
            var infoDb = await _userInfoRepository.FindAsync(t => t.UserId == _user.Id);
            if (infoDb.Count == 0)
            {
                return new UserInfo();
            }

            var info = _mapper.Map<UserInfo>(infoDb);
            return info;
        }
    }
}
