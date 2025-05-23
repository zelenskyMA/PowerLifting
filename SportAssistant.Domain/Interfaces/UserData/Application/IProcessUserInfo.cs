﻿using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Domain.Interfaces.UserData.Application;

public interface IProcessUserInfo
{
    /// <summary>
    /// Получение информации о пользователе по его Id
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="coachInfoRequest">Признак запроса от спортсмена для получения инфы тренера. Для предотвращения зацикливания</param>
    /// <returns></returns>
    Task<UserInfo> GetInfo(int userId, bool coachInfoRequest = true);

    /// <summary>
    /// Получение информации (базовой) по списку пользователей
    /// </summary>
    /// <param name="userIds">Список Ид пользователей</param>
    /// <returns></returns>
    Task<List<UserInfo>> GetInfoList(List<int> userIds);
}
