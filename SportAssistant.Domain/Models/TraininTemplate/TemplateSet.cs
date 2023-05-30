using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.DbModels.TrainingTemplate;

public class TemplateSet : Entity
{
    public int CoachId { get; set; }

    public string Name { get; set; }

    public List<TemplatePlan> Templates { get; set; } = new List<TemplatePlan>();
}
