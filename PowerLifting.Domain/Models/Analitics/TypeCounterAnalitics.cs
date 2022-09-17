namespace PowerLifting.Domain.Models.Analitics
{
    public class TypeCounterAnalitics
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DateValueModel> Values { get; set; } = new List<DateValueModel>();     
    }
}
