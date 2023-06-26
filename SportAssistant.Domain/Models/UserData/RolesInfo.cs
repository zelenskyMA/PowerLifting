namespace SportAssistant.Domain.Models.UserData;

public class RolesInfo
{
    /// <summary>
    /// UI передает значение для обновления прав
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// True, если есть роль администратора
    /// </summary>
    public bool IsAdmin { get; set; } = false;

    /// <summary>
    /// True, если есть роль тренера
    /// </summary>
    public bool IsCoach { get; set; } = false;

    /// <summary>
    /// True, если есть роль менеджера
    /// </summary>
    public bool IsManager { get; set; } = false;

    /// <summary>
    /// True, если есть роль руководителя
    /// </summary>
    public bool IsOrgOwner { get; set; } = false;
}
