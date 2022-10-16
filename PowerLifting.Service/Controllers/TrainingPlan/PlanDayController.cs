using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.TrainingPlan.PlanDayCommands;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers.TrainingPlan
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
