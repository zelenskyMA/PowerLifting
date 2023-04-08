using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.ReportGeneration;

namespace SportAssistant.Application.ReportGeneration;

public class ReportGenerationCommand : ICommand<ReportGenerationCommand.Param, byte[]>
{
    private readonly IDataCollector _dataCollector;
    private readonly IFileCreation _fileCreation;

    public ReportGenerationCommand(
        IDataCollector dataCollector,
        IFileCreation fileCreation)
    {
        _dataCollector = dataCollector;
        _fileCreation = fileCreation;
    }

    public async Task<byte[]> ExecuteAsync(Param param)
    {
        var reportData = await _dataCollector.CollectPlanData(param.PlanId, param.CompletedOnly);

        var report = _fileCreation.Generate(reportData);

        return report;
    }

    public class Param
    {
        public int PlanId { get; set; }

        public bool CompletedOnly { get; set; }
    }
}

