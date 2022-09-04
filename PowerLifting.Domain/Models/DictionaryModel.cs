using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models
{
    public class DictionaryItem : NamedEntity
    {
        public int TypeId { get; set; }

        public string TypeName { get; set; }
    }
}
