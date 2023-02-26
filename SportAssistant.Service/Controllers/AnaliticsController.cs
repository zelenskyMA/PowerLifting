using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Analitics.PlanAnaliticsCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Analitics;

namespace SportAssistant.Service.Controllers
{
    [Route("analitics")]
    public class AnaliticsController : BaseController
    {
        [HttpGet]
        [Route("getPlanAnalitics")]
        [Route("getPlanAnalitics/{userId}")]
        public async Task<PlanAnalitics> GetPlanAnaliticsAsync(
            [FromServices] ICommand<PlanAnaliticsGetQuery.Param, PlanAnalitics> command,
            DateTime startDate, DateTime finishDate, int userId = 0)
        {
            var result = await command.ExecuteAsync(
                new PlanAnaliticsGetQuery.Param() { UserId = userId, StartDate = startDate, FinishDate = finishDate });
            return result;
        }
    }
}