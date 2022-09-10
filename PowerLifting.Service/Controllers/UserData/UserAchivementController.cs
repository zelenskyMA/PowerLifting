using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Service.Controllers.UserData
{
    [ApiController]
    [Route("userAchivement")]
    public class UserAchivementController : ControllerBase
    {
        private readonly IUserAchivementCommands _userAchivementCommands;

        public UserAchivementController(IUserAchivementCommands userAchivementCommands)
        {
            _userAchivementCommands = userAchivementCommands;
        }

        [HttpGet]
        [Route("getByExercise")]
        public async Task<UserAchivement> GetList(int userId, int exerciseTypeId)
        {
            var result = await _userAchivementCommands.GetByExerciseType(userId, exerciseTypeId);
            return result;
        }
    }
}
