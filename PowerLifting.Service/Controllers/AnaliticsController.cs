using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.Analitics.PlanAnaliticsCommands;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models.Analitics;

namespace PowerLifting.Service.Controllers
{
    [Route("analitics")]
    public class AnaliticsController : BaseController
    {
        [HttpGet]
        [Route("getPlanAnalitics")]
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