using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.UserData;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Coaching;

public class TrainingGroupRepository : CrudRepo<TrainingGroupDb>
{
    public TrainingGroupRepository(IContextProvider provider) : base(provider) { }
}