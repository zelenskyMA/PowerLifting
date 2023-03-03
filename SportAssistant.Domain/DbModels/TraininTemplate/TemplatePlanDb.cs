using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingTemplate
{
    [Table("TemplatePlans", Schema = "plan")]
    public class TemplatePlanDb : EntityDb
    {
        public int TemplateSetId { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
