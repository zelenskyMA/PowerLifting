namespace SportAssistant.Domain.Models.ReportGeneration;

public class ReportData
{
    public DateTime PlanStartDate { get; set; } 

    public List<ReportDay> Days { get; set; } = new List<ReportDay>();
}
