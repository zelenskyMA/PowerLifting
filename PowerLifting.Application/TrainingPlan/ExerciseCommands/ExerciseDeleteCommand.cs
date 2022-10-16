using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;

namespace PowerLifting.Application.TrainingPlan.ExerciseCommands
{
    /// <summary>
    /// Delete exercise.
    /// </summary>
    public class ExerciseDeleteCommand : ICommand<ExerciseDeleteCommand.Param, bool>
    {
        private readonly IUserRoleCommands _userRoleCommands;

        private readonly ICrudRepo<ExerciseDb> _exerciseRepository;
        private readonly IUserProvider _user;

        public ExerciseDeleteCommand(
         IUserRoleCommands userRoleCommands,
         ICrudRepo<ExerciseDb> exerciseRepository,
         IUserProvider user)
        {
            _userRoleCommands = userRoleCommands;
            _exerciseRepository = exerciseRepository;
            _user = user;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            var allowedUserIds = new int?[] { null, 0, _user.Id };
            var exercisesDb = await _exerciseRepository.FindAsync(t => t.Id == param.Id);

            if (!exercisesDb.Any())
            {
                throw new BusinessException("Выбранное упражнение не существует");
            }

            var exerciseDb = exercisesDb.First();
            bool isAdmin = await _userRoleCommands.IHaveRole(UserRoles.Admin);
            if (!isAdmin && !(exerciseDb.UserId > 0))
            {
                throw new BusinessException("Базовый справочник упражнений редактируют только администраторы");
            }

            exerciseDb.Closed = true;
            _exerciseRepository.Update(exerciseDb);

            return true;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
