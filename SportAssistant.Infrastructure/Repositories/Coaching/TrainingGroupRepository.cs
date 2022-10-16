using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Coaching
{
    public class TrainingGroupRepository : CrudRepo<TrainingGroupDb>
    {
        public TrainingGroupRepository(IContextProvider provider) : base(provider) { }
    }
}
