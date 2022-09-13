using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Service.Controllers.UserData
{
    [Route("userAchivement")]
    public class UserAchivementController : BaseController
    {
        private readonly IUserAchivementCommands _userAchivementCommands;

        public UserAchivementController(IUserAchivementCommands userAchivementCommands)
        {
            _userAchivementCommands = userAchivementCommands;
        }

        [HttpGet]
        [Route("get")]
        public async Task<List<UserAchivement>> GetList()
        {
            var result = await _userAchivementCommands.GetAsync();
            return result;
        }

        [HttpGet]
        [Route("getByExercise")]
        public async Task<UserAchivement> GetList(int exerciseTypeId)
        {
            var result = await _userAchivementCommands.GetByExerciseTypeAsync(exerciseTypeId);
            return result;
        }
    }
}
