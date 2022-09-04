using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{
    [Table("Dictionaries", Schema = "dbo")]
    public class DictionaryDb : NamedEntityDb
    {
        public int TypeId { get; set; }
    }
}
