using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Common.Actions;

namespace PowerLifting.Application.Coaching.TrainingRequestCommands
{
    /// <summary>
    /// Удаление заявки тренером. Отказ в обучении
    /// </summary>
    public class TrainingRequestRemoveCommand : ICommand<TrainingRequestRemoveCommand.Param, bool>
    {
        private readonly IProcessTrainingRequest _processTrainingRequest;

        public TrainingRequestRemoveCommand(
            IProcessTrainingRequest processTrainingRequest)
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
