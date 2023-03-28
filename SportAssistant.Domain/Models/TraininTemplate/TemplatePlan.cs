using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingTemplate;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Domain.DbModels.TrainingTemplate
{
    public class TemplatePlan : Entity
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public List<TemplateDay> TrainingDays { get; set; } = new List<TemplateDay>();

        public CountersTemplatePlan Counters { get; set; } = new CountersTemplatePlan();
    }
}
