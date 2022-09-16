using PowerLifting.Domain.Models.Analitics;

namespace PowerLifting.Domain.Interfaces.Analitics.Application
{
    public interface IPlanAnaliticsCommands
    {
        /// <summary>
        /// Get plan counters for selected period
        /// </summary>
        /// <param name="startDate">Period start date</param>
        /// <param name="finishDate">Period end date</param>
        /// <returns></returns>
        Task<List<PlanDateAnaliticsData>> GetAsync(DateTime startDate, DateTime finishDate);
    }
}
