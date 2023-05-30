using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TrainingTemplate;

public class TemplateSetRepository : CrudRepo<TemplateSetDb>
{
    public TemplateSetRepository(IContextProvider provider) : base(provider) { }
}
