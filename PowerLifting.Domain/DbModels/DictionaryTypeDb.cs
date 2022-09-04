using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{
    [Table("DictionaryTypes", Schema = "dbo")]
    public class DictionaryTypeDb : NamedEntityDb
    {
    }
}
