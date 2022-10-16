using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.TrainingPlan.PlanCommands;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers.TrainingPlan
{
    [Route("trainingPlan")]
    public class PlanController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<Plan> GetAsync([FromServices] ICommand<PlanGetByIdQuery.Param, Plan> command, int id)
        {
            var result = await command.ExecuteAsync(new PlanGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpGet]
        [Route("getList")]
        public async Task<Plans> GetListAsync([FromServices] ICommand<PlanGetListQuery.Param, Plans> command, int userId = 0)
        {
            var result = await command.ExecuteAsync(new PlanGetListQuery.Param() { UserId = userId });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<int> CreateAsync([FromServices] ICommand<PlanCreateCommand.Param, int> command, PlanCreateCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }       
    }
}
