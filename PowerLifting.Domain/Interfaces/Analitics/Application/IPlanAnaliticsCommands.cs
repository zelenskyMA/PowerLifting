using PowerLifting.Domain.Models.Analitics;
using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Interfaces.Analitics.Application
{
    public interface IPlanAnaliticsCommands
    {
        /// <summary>
        /// Get plan counters for selected period
        /// </summary>
        /// <param name="span">Request period with start and finish date</param>
        /// <returns></returns>
        Task<PlanAnalitics> GetAsync(TimeSpanEntity span);
    }
}
