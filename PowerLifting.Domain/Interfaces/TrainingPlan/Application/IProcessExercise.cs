using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessExercise
    {
        /// <summary>
        /// Get exercise by id. Closed allowed for back compatibility.
        /// For plans only.
        /// </summary>
        /// <param name="id">Exercise Id</param>
        /// <returns></returns>
        Task<Exercise?> GetAsync(int id);

        /// <summary>
        /// Get exercises by id list. Closed allowed for back compatibility.
        /// For plans only.
        /// </summary>
        /// <param name="ids">Exercise Ids</param>
        /// <returns></returns>
        Task<List<Exercise>> GetAsync(List<int> ids);

        /// <summary>
        /// Get exercises, allowed for supplied user Ids
        /// </summary>
        /// <param name="allowedUserIds"></param>
        /// <returns></returns>
        Task<List<Exercise>> GetAllowedExercises(int?[] allowedUserIds);

        /// <summary>
        /// Записываем справочные данные в модель упражнения
        /// </summary>
        /// <param name="exercises"></param>
        /// <returns></returns>
        Task SetDictionaryData(List<Exercise> exercises);
    }
}
