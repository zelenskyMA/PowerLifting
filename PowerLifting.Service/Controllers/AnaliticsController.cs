using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.Analitics.Application;
using PowerLifting.Domain.Models.Analitics;

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
        public async Task<List<PlanDateAnaliticsData>> GetPlanAnaliticsAsync(DateTime startDate, DateTime finishDate)
        {
            var result = await _planAnaliticsCommands.GetAsync(startDate, finishDate);
            return result;
        }
    }
}