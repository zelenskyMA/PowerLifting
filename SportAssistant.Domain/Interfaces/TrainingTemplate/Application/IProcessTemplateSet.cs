namespace SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

public interface IProcessTemplateSet
{
    /// <summary>
    /// Проверка допустимости создания / обновления цикла текущим пользователем.
    /// </summary>
    /// <param name="coachIdForCheck">Ид тренера, которому принадлежит цикл</param>
    /// <returns></returns>
    Task<int> ChangingAllowedForUserAsync(int coachIdForCheck);

    /// <summary>
    /// Проверка допустимости просмотра данных пользователя coachIdForCheck текущим пользователем.
    /// </summary>
    /// <param name="coachIdForCheck">Ид тренера, которому принадлежит цикл</param>
    /// <returns></returns>
    Task<bool> ViewAllowedForDataOfUserAsync(int coachIdForCheck);
}
