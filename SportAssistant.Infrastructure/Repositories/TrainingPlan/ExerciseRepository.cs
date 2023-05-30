using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingPlan;

public class ExerciseRepository : CrudRepo<ExerciseDb>
{
    public ExerciseRepository(IContextProvider provider) : base(provider)
    {
    }
}
