using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserInfoCommands;

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
