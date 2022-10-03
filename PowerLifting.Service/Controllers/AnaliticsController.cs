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
        public async Task<PlanAnalitics> GetPlanAnaliticsAsync(DateTime startDate, DateTime finishDate, int userId = 0)
        {
            var result = await _planAnaliticsCommands.GetAsync(userId,
                new TimeSpanEntity() { StartDate = startDate, FinishDate = finishDate });

            return result;
        }
    }
}