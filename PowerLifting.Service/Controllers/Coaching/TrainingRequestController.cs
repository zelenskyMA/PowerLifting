using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Service.Controllers.Coaching
{

    [Route("trainingRequests")]
    public class TrainingRequestController : BaseController
    {
        private readonly ITrainingRequestCommands _trainingRequestCommands;

        public TrainingRequestController(ITrainingRequestCommands trainingRequestCommands)
        {
            _trainingRequestCommands = trainingRequestCommands;
        }

        [HttpGet]
        [Route("getMyRequest")]
        public async Task<TrainingRequest> GetMyRequestAsync()
        {
            var result = await _trainingRequestCommands.GetMyRequestAsync();
            return result;
        }

        [HttpGet]
        [Route("getCoachRequests")]
        public async Task<List<TrainingRequest>> GetCoachRequestsAsync()
        {
            var result = await _trainingRequestCommands.GetCoachRequestsAsync();
            return result;
        }

        [HttpGet]
        [Route("getCoaches")]
        public async Task<List<CoachInfo>> GetCoachesAsync()
        {
            var result = await _trainingRequestCommands.GetCoachesAsync();
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync(int coachId)
        {
            await _trainingRequestCommands.CreateAsync(coachId);
            return true;
        }

        [HttpPost]
        [Route("remove")]
        public async Task<bool> RemoveAsync(int userId = 0)
        {
            await _trainingRequestCommands.RemoveRequestAsync(userId);
            return true;
        }
    }
}
