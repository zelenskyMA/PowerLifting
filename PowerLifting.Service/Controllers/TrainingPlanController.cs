using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Service.Controllers
{

    [ApiController]
    [Route("trainingPlan")]
    public class TrainingPlanController : ControllerBase
    {
        private readonly ITrainingPlanCommands _trainingPlanApp;

        public TrainingPlanController(ITrainingPlanCommands trainingPlanApp)
        {
            _trainingPlanApp = trainingPlanApp;
        }

        [HttpGet]
        [Route("get")]
        public async Task<TrainingPlanModel> Get(int id)
        {
            return await _trainingPlanApp.GetAsync(id);
        }

        [HttpPost]
        [Route("create")]
        public async Task<int> Create(DateTime creationDate)
        {
            var result = await _trainingPlanApp.CreateAsync(creationDate);
            return result;
        }
    }
}
