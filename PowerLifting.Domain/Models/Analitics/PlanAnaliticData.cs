using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.Analitics
{
    public class PlanDateAnaliticsData
    {
        public DateTime PlanStartDate { get; set; }

        public DateTime PlanFinishDate { get; set; }

        public int LiftCounterSum { get; set; }

        public int WeightLoadSum { get; set; }

        public int IntensitySum { get; set; }

        public List<ValueEntity> TypeCountersSum { get; set; } = new List<ValueEntity>();
    }
}
