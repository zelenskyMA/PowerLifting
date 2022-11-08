using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportAssistant.Domain.Interfaces.TrainingPlan.Application
{
    public interface IProcessPlanUserId
    {
        /// <summary>
        /// Получение Ид спортсмена по Ид дня в плане
        /// </summary>
        /// <param name="id">Ид тренировочного дня</param>
        /// <returns></returns>
        Task<int> GetByDayId(int id);

        /// <summary>
        /// Получение Ид спортсмена по Ид упражнения в плане
        /// </summary>
        /// <param name="id">Ид запланированного упражнения</param>
        /// <returns></returns>
        Task<int> GetByPlanExerciseId(int id);
    }
}
