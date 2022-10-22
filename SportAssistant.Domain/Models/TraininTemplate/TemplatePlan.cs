using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Domain.DbModels.TraininTemplate
{
    public class TemplatePlan : Entity
    {
        public string Name { get; set; }

        public List<TemplateDay> Days { get; set; } = new List<TemplateDay>();
    }
}
