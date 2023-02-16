using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.Coaching.TrainingGroupUserCommands
{
    /// <summary>
    /// Отмена заявки спортсменом или отказ тренера.
    /// </summary>
    public class GroupUserRejectCommand : ICommand<GroupUserRejectCommand.Param, bool>
    {
        private readonly IProcessRequest _processTrainingRequest;
        private readonly ITrainingGroupUserRepository _trainingGroupUserRepository;
        private readonly ICrudRepo<UserInfoDb> _userInfoRepository;
        private readonly IUserProvider _user;

        public GroupUserRejectCommand(
         IProcessRequest processTrainingRequest,
         ITrainingGroupUserRepository trainingGroupUserRepository,
         ICrudRepo<UserInfoDb> userInfoRepository,
         IUserProvider user)
        {
            _processTrainingRequest = processTrainingRequest;
            _trainingGroupUserRepository = trainingGroupUserRepository;
            _userInfoRepository = userInfoRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var userInfoDb = (await _userInfoRepository.FindAsync(t => t.UserId == _user.Id)).FirstOrDefault();
            if (userInfoDb == null)
            {
                throw new BusinessException("Пользователь не найден");
            }

            var userGroupDb = (await _trainingGroupUserRepository.FindAsync(t => t.UserId == _user.Id)).FirstOrDefault();
            if (userGroupDb != null)
            {
                _trainingGroupUserRepository.Delete(userGroupDb);
            }

            await _processTrainingRequest.RemoveAsync(_user.Id);

            userInfoDb.CoachId = null;
            _userInfoRepository.Update(userInfoDb);

            return true;
        }

        public class Param { }
    }
}
