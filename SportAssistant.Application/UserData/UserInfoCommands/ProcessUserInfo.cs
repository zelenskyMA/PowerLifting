using AutoMapper;
using SportAssistant.Application.Common.Actions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserInfoCommands;

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
    public async Task<UserInfo> GetInfo(int userId, bool coachInfoRequest = true)
    {
        var infoDb = await _userInfoRepository.FindOneAsync(t => t.UserId == userId);
        if (infoDb == null)
        {
            return new UserInfo();
        }

        var info = _mapper.Map<UserInfo>(infoDb);
        info.LegalName = UserNaming.GetLegalShortName(info.FirstName, info.Surname, info.Patronimic, "Кабинет");
        info.RolesInfo = await _userRoleCommands.GetUserRoles(userId);

        if (info.CoachId > 0 && coachInfoRequest)
        {
            info.CoachLegalName = (await GetInfo(info.CoachId.Value, false)).LegalName;
        }

        return info;
    }

    /// <inheritdoc />
    public async Task<List<UserInfo>> GetInfoList(List<int> userIds)
    {
        var infoDbList = await _userInfoRepository.FindAsync(t => userIds.Contains(t.UserId));
        
        var infoList = infoDbList.Select(_mapper.Map<UserInfo>).ToList();
        foreach (var info in infoList)
        {
            info.LegalName = UserNaming.GetLegalFullName(info, "Имя не задано");
        }

        return infoList;
    }
}
