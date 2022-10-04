using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.Coaching;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Service.Controllers.Coaching
{
    [Route("trainingGroups")]
    public class TrainingGroupController : BaseController
    {
        private readonly ITrainingGroupCommands _trainingGroupCommands;
        private readonly IUserTrainingGroupCommands _userTrainingGroupCommands;

        public TrainingGroupController(ITrainingGroupCommands trainingGroupCommands, IUserTrainingGroupCommands userTrainingGroupCommands)
        {
            _trainingGroupCommands = trainingGroupCommands;
            _userTrainingGroupCommands = userTrainingGroupCommands;
        }

        [HttpGet]
        [Route("getList")]
        public async Task<List<TrainingGroup>> GetCoachGroupsAsync()
        {
            var result = await _trainingGroupCommands.GetListAsync();
            return result;
        }

        [HttpGet]
        [Route("get")]
        public async Task<TrainingGroupInfo> GetGroupInfoAsync(int id)
        {
            var result = await _trainingGroupCommands.GetAsync(id);
            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync(TrainingGroup group)
        {
            await _trainingGroupCommands.CreateAsync(group);
            return true;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> UpdateAsync(TrainingGroup group)
        {
            await _trainingGroupCommands.UpdateAsync(group);
            return true;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<bool> DeleteAsync(int id)
        {
            await _trainingGroupCommands.DeleteAsync(id);
            return true;
        }

        [HttpPost]
        [Route("rejectCoach")]
        public async Task<bool> RejectCoach() {
            await _userTrainingGroupCommands.RejectCoach();
            return true;
        }

        [HttpPost]
        [Route("updateUserGroup")]
        public async Task<bool> UpdateUserGroupAsync(UserTrainingGroup targetGroup)
        {
            await _userTrainingGroupCommands.UpdateUserGroup(targetGroup);
            return true;
        }

        [HttpPost]
        [Route("removeFromGroup")]
        public async Task<bool> RemoveUserFromGroup(UserTrainingGroup targetGroup)
        {
            await _userTrainingGroupCommands.RemoveUserFromGroup(targetGroup);
            return true;
        }

    }
}