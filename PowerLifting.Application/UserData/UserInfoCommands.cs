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
            var infoDb = (await _userInfoRepository.FindAsync(t => t.UserId == _user.Id)).FirstOrDefault();
            if (infoDb == null)
            {
                return new UserInfo();
            }

            var info = _mapper.Map<UserInfo>(infoDb);

            string patronimic = string.IsNullOrEmpty(info.Patronimic) ? string.Empty : $" {info.Patronimic.ToUpper().First()}.";
            string firstName = string.IsNullOrEmpty(info.FirstName) ? string.Empty : $" {info.FirstName?.ToUpper()?.First()}.";
            info.LegalName = string.IsNullOrEmpty(info.Surname) ? "Кабинет" : $"{info.Surname}{firstName}{patronimic}";

            return info;
        }

        public async Task UpdateAsync(UserInfo userInfo)
        {
            var userInfoDb = _mapper.Map<UserInfoDb>(userInfo);
            userInfoDb.UserId = _user.Id;

            await _userInfoRepository.UpdateAsync(userInfoDb);
        }
    }
}
