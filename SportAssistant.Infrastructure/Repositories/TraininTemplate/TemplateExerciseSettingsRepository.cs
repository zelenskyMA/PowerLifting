using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan
{
    public class TemplateExerciseSettingsRepository : CrudRepo<TemplateExerciseSettingsDb>
    {
        public TemplateExerciseSettingsRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
