namespace SportAssistant.Domain.Models.Analitics
{
    public class CategoryCounterAnalitics
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DateValueModel> Values { get; set; } = new List<DateValueModel>();
    }
}
