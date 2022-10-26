using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan
{
    public class PercentageRepository : CrudRepo<PercentageDb>
    {
        public PercentageRepository(IContextProvider provider) : base(provider)
        {
        }
    }
}
