namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessPlan
    {
        /// <summary>
        /// Проверка допустимости создавать / менять план пользователя текущему пользователю.
        /// </summary>
        /// <param name="userIdForCheck">Ид пользователя, которому принадлежит план</param>
        /// <returns></returns>
        Task<int> PlanningAllowedForUserAsync(int userIdForCheck);
    }
}
