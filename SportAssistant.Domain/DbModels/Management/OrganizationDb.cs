using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Management;

[Table("Organizations", Schema = "org")]
public class OrganizationDb : NamedEntityDb
{   
    public int OwnerId { get; set; }

    public int MaxCoaches { get; set; }
}
