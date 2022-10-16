using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models.Coaching;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Получение заявки по Ид пользователя.
    /// </summary>
    public class TrainingRequestGetByUserQuery : ICommand<TrainingRequestGetByUserQuery.Param, TrainingRequest>
    {
        private readonly IProcessTrainingRequest _processTrainingRequest;

        public TrainingRequestGetByUserQuery(
            IProcessTrainingRequest processTrainingRequest)
        {
            _processTrainingRequest = processTrainingRequest;
        }

        public async Task<TrainingRequest> ExecuteAsync(Param param)
        {
            var request = await _processTrainingRequest.GetByUserAsync(param.UserId);
            return request;
        }

        public class Param
        {
            public int UserId { get; set; }
        }
    }
}
