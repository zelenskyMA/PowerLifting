using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.TraininTemplate
{
    public class TemplateSetRepository : CrudRepo<TemplateSetDb>
    {
        public TemplateSetRepository(IContextProvider provider) : base(provider) { }
    }
}
