using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Basic;

public class EmailMessageRepository : CrudRepo<EmailMessageDb>
{
    public EmailMessageRepository(IContextProvider provider) : base(provider) { }
}
