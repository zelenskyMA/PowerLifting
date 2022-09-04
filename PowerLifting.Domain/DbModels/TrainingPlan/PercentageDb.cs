using PowerLifting.Domain.DbModels.Common;

namespace PowerLifting.Domain.DbModels.TrainingPlan
{
    public class PercentageDb : NamedEntityDb
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}
