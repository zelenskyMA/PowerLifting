using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Basic
{
    [Table("Settings", Schema = "dbo")]
    public class SettingsDb : NamedEntityDb
    {
        public string? Value { get; set; }
    }
}
