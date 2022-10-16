using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.TrainingPlan
{
    public class Percentage : NamedEntity
    {
        public int MinValue { get; set; } = 0;

        public int MaxValue { get; set; } = 0;
    }
}
