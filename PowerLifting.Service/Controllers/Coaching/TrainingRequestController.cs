using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.Coaching.TrainingRequestCommands;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Service.Controllers.Coaching
{

    [Route("trainingRequests")]
    public class TrainingRequestController : BaseController
    {
        private readonly IUserProvider _user;

        public TrainingRequestController(IUserProvider user)
        {
            _user = user;
        }

        [HttpGet]
        [Route("getMyRequest")]
        public async Task<TrainingRequest> GetMyRequestAsync([FromServices] ICommand<TrainingRequestGetByUserQuery.Param, TrainingRequest> command)
        {
            var result = await command.ExecuteAsync(new TrainingRequestGetByUserQuery.Param() { UserId = _user.Id });
            return result;
        }

        [HttpGet]
        [Route("getCoachRequests")]
        public async Task<List<TrainingRequest>> GetCoachRequestsAsync([FromServices] ICommand<TrainingRequestGetForCoachQuery.Param, List<TrainingRequest>> command)
        {
            var result = await command.ExecuteAsync(new TrainingRequestGetForCoachQuery.Param() { });
            return result;
        }

        [HttpGet]
        [Route("getCoaches")]
        public async Task<List<CoachInfo>> GetCoachesAsync([FromServices] ICommand<TrainingRequestGetAvailableCoachesQuery.Param, List<CoachInfo>> command)
        {
            var result = await command.ExecuteAsync(new TrainingRequestGetAvailableCoachesQuery.Param() { });
            return result;
        }

        [HttpGet]
        [Route("get")]
        public async Task<TrainingRequest> GetAsync([FromServices] ICommand<TrainingRequestGetForCoachSingleQuery.Param, TrainingRequest> command, int id)
        {
            var result = await command.ExecuteAsync(new TrainingRequestGetForCoachSingleQuery.Param() { Id = id });
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync([FromServices] ICommand<TrainingRequestCreateCommand.Param, bool> command, int coachId)
        {
            var result = await command.ExecuteAsync(new TrainingRequestCreateCommand.Param() { СoachId = coachId });
            return result;
        }

        [HttpPost]
        [Route("remove")]
        public async Task<bool> RemoveAsync([FromServices] ICommand<TrainingRequestRemoveCommand.Param, bool> command, int userId = 0)
        {
            var result = await command.ExecuteAsync(new TrainingRequestRemoveCommand.Param() { UserId = userId });
            return result;
        }
    }
}
