using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Common.Operations;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Удаление заявки тренером. Отказ в обучении
    /// </summary>
    public class RequestRemoveCommand : ICommand<RequestRemoveCommand.Param, bool>
    {
        private readonly IProcessRequest _processTrainingRequest;

        public RequestRemoveCommand(
            IProcessRequest processTrainingRequest)
        {
            _processTrainingRequest = processTrainingRequest;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            await _processTrainingRequest.RemoveAsync(param.UserId);
            return true;
        }

        public class Param
        {
            public int UserId { get; set; }
        }
    }
}
