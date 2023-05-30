using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class TemplateExerciseRepository : CrudRepo<TemplateExerciseDb>
{
    public TemplateExerciseRepository(IContextProvider provider) : base(provider)
    {
    }
}
