using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Basic;

[Table("Dictionaries", Schema = "dbo")]
public class DictionaryDb : NamedEntityDb
{
    public int TypeId { get; set; }
}
