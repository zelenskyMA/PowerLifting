using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.Coaching.TrainingGroupCommands;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Service.Controllers.Coaching
{
    [Route("trainingGroups")]
    public class TrainingGroupController : BaseController
    {
        [HttpGet]
        [Route("getList")]
        public async Task<List<TrainingGroup>> GetListAsync([FromServices] ICommand<TrainingGroupGetListQuery.Param, List<TrainingGroup>> command)
        {
            var result = await command.ExecuteAsync(new TrainingGroupGetListQuery.Param() { });
            return result;
        }

        [HttpGet]
        [Route("get")]
        public async Task<TrainingGroupInfo> GetAsync([FromServices] ICommand<TrainingGroupGetByIdQuery.Param, TrainingGroupInfo> command, int id)
        {
            var result = await command.ExecuteAsync(new TrainingGroupGetByIdQuery.Param() { Id = id });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync([FromServices] ICommand<TrainingGroupCreateCommand.Param, bool> command, TrainingGroup group)
        {
            var result = await command.ExecuteAsync(new TrainingGroupCreateCommand.Param() { Group = group });
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync([FromServices] ICommand<TrainingGroupUpdateCommand.Param, bool> command, TrainingGroup group)
        {
            var result = await command.ExecuteAsync(new TrainingGroupUpdateCommand.Param() { Group = group });
            return result;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<bool> DeleteAsync([FromServices] ICommand<TrainingGroupDeleteCommand.Param, bool> command, int id)
        {
            var result = await command.ExecuteAsync(new TrainingGroupDeleteCommand.Param() { Id = id });
            return result;
        }
    }
}