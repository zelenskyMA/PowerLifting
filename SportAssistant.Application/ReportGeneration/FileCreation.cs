using SportAssistant.Domain.Interfaces.ReportGeneration;
using SportAssistant.Domain.Models.ReportGeneration;

namespace SportAssistant.Application.ReportGeneration;

public class FileCreation : IFileCreation
{
    /// <inheritdoc />
    public byte[] Generate(ReportData report)
    {
        return new byte[0];
    }
}
