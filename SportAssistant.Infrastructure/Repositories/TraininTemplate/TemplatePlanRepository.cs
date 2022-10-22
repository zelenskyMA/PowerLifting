using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan
{
    public class TemplatePlanRepository : CrudRepo<TemplatePlanDb>
    {
        public TemplatePlanRepository(IContextProvider provider) : base(provider) { }
    }
}
