using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

namespace SportAssistant.Application.TraininTemplate.TemplateSetCommands
{
    public class ProcessTemplateSet : IProcessTemplateSet
    {
        private readonly IUserProvider _user;

        public ProcessTemplateSet(
            IUserProvider user)
        {
            _user = user;
        }

        /// <inheritdoc />
        public async Task<int> ChangingAllowedForUserAsync(int coachIdForCheck)
        {
            if (coachIdForCheck != 0 && coachIdForCheck != _user.Id)
            {
                throw new BusinessException("У вас нет права изменять данные в выбранном тренировочном цикле");
            }

            return coachIdForCheck;
        }

        /// <inheritdoc />
        public async Task<bool> ViewAllowedForDataOfUserAsync(int coachIdForCheck)
        {
            if (coachIdForCheck != 0 && coachIdForCheck != _user.Id)
            {
                throw new DataException();
            }

            return true;
        }
    }
}
