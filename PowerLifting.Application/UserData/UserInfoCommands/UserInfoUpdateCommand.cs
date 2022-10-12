using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.UserData.UserInfoCommands
{
    public class UserInfoUpdateCommand : ICommand<UserInfoUpdateCommand.Param, bool>
    {
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public UserInfoUpdateCommand(
            ICrudRepo<UserInfoDb> userInfoRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
            _user = user;
        }

        /// <inheritdoc />       
        public async Task<bool> ExecuteAsync(Param param)
        {
            var userInfoDb = _mapper.Map<UserInfoDb>(param.Info);
            userInfoDb.UserId = _user.Id;

            _userInfoRepository.Update(userInfoDb);

            return true;
        }

        public class Param
        {
            public UserInfo Info { get; set; }
        }
    }
}
