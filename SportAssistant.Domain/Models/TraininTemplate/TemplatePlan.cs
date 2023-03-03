using SportAssistant.Domain.Models.Common;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Domain.DbModels.TrainingTemplate
{
    public class TemplatePlan : Entity
    {
        public string Name { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// Расчетные данные. Количество упражнений по подтипам в сумме за все дни плана.
        /// </summary>
        public List<ValueEntity> TypeCountersSum { get; set; } = new List<ValueEntity>();

        public List<TemplateDay> TrainingDays { get; set; } = new List<TemplateDay>();
    }
}
