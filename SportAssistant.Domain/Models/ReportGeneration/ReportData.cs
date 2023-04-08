namespace SportAssistant.Domain.Models.ReportGeneration;

public class ReportData
{
    public DateTime PlanStartDate { get; set; }

    public string PlanOwner { get; set; } = string.Empty;

    public List<ReportDay> Days { get; set; } = new List<ReportDay>();
}
