using System.ComponentModel.DataAnnotations;

namespace SportAssistant.Domain.DbModels.Common;

public class EntityDb
{
    [Key]
    public int Id { get; set; }
}
