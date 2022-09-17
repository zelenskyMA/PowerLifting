using static PowerLifting.Domain.Models.Analitics.TypeCounterAnalitics;

namespace PowerLifting.Domain.Models.Analitics
{
    public class PlanAnalitics
    {
        /// <summary>
        /// Plan counter data, sorted by plan dates
        /// </summary>
        public List<PlanCounterAnalitics> PlanCounters { get; set; } = new List<PlanCounterAnalitics>();

        /// <summary>
        /// plan exercise subTypes counters, solted by plan dates
        /// </summary>
        public List<TypeCounterAnalitics> TypeCounters { get; set; } = new List<TypeCounterAnalitics>();

        /// <summary>
        /// Contains entries for each plan in analitics set. Used as full dataset for chart data selection.
        /// All real data will be taken from TypeCounters by entries, presented here.
        /// </summary>
        public List<DateValueModel> FullTypeCounterList { get; set; } = new List<DateValueModel>();
    }
}
