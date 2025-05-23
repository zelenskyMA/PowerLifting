﻿using SportAssistant.Application.Common.Actions;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserInfoCommands;

public class UserInfoGetCardQuery : ICommand<UserInfoGetCardQuery.Param, UserCard>
{
    private readonly IUserBlockCommands _userBlockCommands;
    private readonly IUserRoleCommands _userRoleCommands;
    private readonly IProcessUserAchivements _processUserAchivements;
    private readonly IProcessGroup _processTrainingGroups;
    private readonly IProcessUserInfo _processUserInfo;
    private readonly ICrudRepo<UserDb> _userRepository;
    private readonly IUserProvider _user;

    public UserInfoGetCardQuery(
        IUserBlockCommands userBlockCommands,
        IUserRoleCommands userRoleCommands,
        IProcessUserAchivements processUserAchivements,
        IProcessGroup processTrainingGroups,
        IProcessUserInfo processUserInfo,
        ICrudRepo<UserDb> userRepository,
        IUserProvider user)
    {
        _userBlockCommands = userBlockCommands;
        _userRoleCommands = userRoleCommands;
        _processUserAchivements = processUserAchivements;
        _processTrainingGroups = processTrainingGroups;

        _processUserInfo = processUserInfo;

        _userRepository = userRepository;
        _user = user;
    }


    /// <inheritdoc />
    public async Task<UserCard> ExecuteAsync(Param param)
    {
        UserDb? userDb = null;
        if (param.UserId != 0)
        {
            userDb = (await _userRepository.FindAsync(t => t.Id == param.UserId)).FirstOrDefault();
        }

        if (!string.IsNullOrEmpty(param.Login) && userDb == null)
        {
            userDb = (await _userRepository.FindAsync(t => t.Email == param.Login)).FirstOrDefault();
        }

        if (userDb == null)
        {
            throw new BusinessException("Пользователь не найден.");
        }

        var info = await _processUserInfo.GetInfo(userDb.Id);

        if (!await CanSeeFullInfoAsync(info, userDb))
        {
            return new UserCard()
            {
                UserId = userDb.Id,
                Login = userDb.Email,
                UserName = UserNaming.GetLegalFullName(info),
            };
        }

        var card = new UserCard()
        {
            UserId = userDb.Id,
            UserName = UserNaming.GetLegalFullName(info),
            Login = userDb.Email,
            BaseInfo = info,
            GroupInfo = await _processTrainingGroups.GetUserGroupAsync(userDb.Id),
            Achivements = await _processUserAchivements.GetAsync(userDb.Id)
        };

        if (userDb.Blocked)
        {
            card.BlockReason = await _userBlockCommands.GetCurrentBlockReason(userDb.Id);
        }

        return card;
    }

    private async Task<bool> CanSeeFullInfoAsync(UserInfo info, UserDb userDb)
    {
        // проверка прав на просмотр полной информации
        if (await _userRoleCommands.IHaveRole(UserRoles.Admin) || // админ
            info.CoachId == _user.Id || // тренер юзера
            userDb.Id == _user.Id) // мой профиль
        {
            return true;
        }

        // частичная, поисковая информация
        if (await _userRoleCommands.IHaveAnyRoles(new List<UserRoles>() { UserRoles.Manager, UserRoles.OrgOwner }))
        {
            return false;
        }

        throw new BusinessException("Нет прав для просмотра данной информации");
    }

    public class Param
    {
        public int UserId { get; set; }

        public string? Login { get; set; }
    }
}
