using PowerLifting.Domain.Models.Analitics;
using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Interfaces.Analitics.Application
{
    public interface IPlanAnaliticsCommands
    {
        /// <summary>
        /// Get plan counters for selected period
        /// </summary>
        /// <param name="userId">Optional user Id for coach data request</param>
        /// <param name="span">Request period with start and finish date</param>
        /// <returns></returns>
        Task<PlanAnalitics> GetAsync(int userId, TimeSpanEntity span);
    }
}
