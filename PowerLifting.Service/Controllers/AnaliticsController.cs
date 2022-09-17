using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.Analitics.Application;
using PowerLifting.Domain.Models.Analitics;
using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Service.Controllers
{
    [Route("analitics")]
    public class AnaliticsController : BaseController
    {
        private readonly IPlanAnaliticsCommands _planAnaliticsCommands;

        public AnaliticsController(IPlanAnaliticsCommands planAnaliticsCommands)
        {
            _planAnaliticsCommands = planAnaliticsCommands;
        }

        [HttpGet]
        [Route("getPlanAnalitics")]
        public async Task<List<PlanDateAnalitics>> GetPlanAnaliticsAsync(DateTime startDate, DateTime finishDate)
        {
            var result = await _planAnaliticsCommands.GetAsync(
                new TimeSpanEntity() { StartDate = startDate, FinishDate = finishDate });

            return result;
        }
    }
}