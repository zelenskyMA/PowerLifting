using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class TemplateDayRepository : CrudRepo<TemplateDayDb>
{
    public TemplateDayRepository(IContextProvider provider) : base(provider) { }
}
