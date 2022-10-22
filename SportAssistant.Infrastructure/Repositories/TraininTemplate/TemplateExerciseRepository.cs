using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan
{
    public class TemplateExerciseRepository : CrudRepo<TemplateExerciseDb>
    {
        public TemplateExerciseRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
