namespace SportAssistant.Domain.Models.Analitics
{
    public class PlanCounterAnalitics
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public int LiftCounterSum { get; set; }

        public int WeightLoadSum { get; set; }

        public int IntensitySum { get; set; }
    }
}