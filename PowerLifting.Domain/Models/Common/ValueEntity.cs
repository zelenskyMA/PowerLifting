namespace PowerLifting.Domain.Models.Common
{
    public class ValueEntity : Entity
    {
        public string? Name { get; set; }

        public int Value { get; set; } = 0;
    }
}
