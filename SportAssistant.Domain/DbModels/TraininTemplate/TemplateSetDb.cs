using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingTemplate;

[Table("TemplateSets", Schema = "plan")]
public class TemplateSetDb : EntityDb
{
    public int CoachId { get; set; }

    public string Name { get; set; }
}
