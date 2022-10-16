using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.TrainingPlan.PlanDayCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Service.Controllers.TrainingPlan
{
    [Route("planDay")]
    public class PlanDayController : BaseController
    {       
        [HttpGet]
        [Route("get")]
        public async Task<PlanDay> GetAsync([FromServices] ICommand<PlanDayGetByIdQuery.Param, PlanDay> command, int id)
        {
            var result = await command.ExecuteAsync(new PlanDayGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getCurrent")]
        public async Task<PlanDay> GetCurrentAsync([FromServices] ICommand<PlanDayGetCurrentQuery.Param, PlanDay> command)
        {
            var result = await command.ExecuteAsync(new PlanDayGetCurrentQuery.Param() { });
            return result;
        }
    }
}
