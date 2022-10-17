using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Basic
{
    [Table("DictionaryTypes", Schema = "dbo")]
    public class DictionaryTypeDb : NamedEntityDb
    {
    }
}
