namespace PowerLifting.Domain.Models.Analitics
{
    public class DateValueModel
    {
        /// <summary>
        /// Date converted to string in correct format. Name is taken by Rechart as key.
        /// </summary>
        public string Name { get; set; }

        public int Value { get; set; } = 0;
    }
}
