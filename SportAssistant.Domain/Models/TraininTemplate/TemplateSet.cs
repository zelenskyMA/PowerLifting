using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.DbModels.TraininTemplate
{
    public class TemplateSet : Entity
    {
        public int CoachId { get; set; }

        public string Name { get; set; }

        public List<TemplatePlan> Plans { get; set; } = new List<TemplatePlan>();
    }
}
