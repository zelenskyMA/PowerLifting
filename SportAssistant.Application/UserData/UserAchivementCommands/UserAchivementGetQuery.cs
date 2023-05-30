using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.UserData.UserAchivementCommands;

/// <summary>
/// Получение личных рекордов спортсмена по всем типам упражнений.
/// </summary>
public class UserAchivementGetQuery : ICommand<UserAchivementGetQuery.Param, List<UserAchivement>>
{
    private readonly IProcessUserAchivements _processUserAchivements;

    public UserAchivementGetQuery(
        IProcessUserAchivements processUserAchivements)
    {
        _processUserAchivements = processUserAchivements;
    }

    /// <inheritdoc />
    public async Task<List<UserAchivement>> ExecuteAsync(Param param)
    {
        var info = await _processUserAchivements.GetAsync(0); // Ид пользователя подставится в process части
        return info;
    }

    public class Param { }
}
