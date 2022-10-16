using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Domain.Models.TrainingPlan;

namespace SportAssistant.Application.TrainingPlan.ExerciseCommands
{
    /// <summary>
    /// Create / Update exercise.
    /// </summary>
    public class ExerciseUpdateCommand : ICommand<ExerciseUpdateCommand.Param, bool>
    {
        private readonly IUserRoleCommands _userRoleCommands;

        private readonly ICrudRepo<ExerciseDb> _exerciseRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public ExerciseUpdateCommand(
         IUserRoleCommands userRoleCommands,
         ICrudRepo<ExerciseDb> exerciseRepository,
         IUserProvider user,
         IMapper mapper)
        {
            _userRoleCommands = userRoleCommands;
            _exerciseRepository = exerciseRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Param param)
        {
            Validate(param.Exercise);

            var allowedUserIds = new int?[] { null, 0, _user.Id };
            var exercisesDb = await _exerciseRepository.FindAsync(t => allowedUserIds.Contains(t.UserId) && !t.Closed);

            if (exercisesDb.Any(t => t.Id != param.Exercise.Id && t.Name == param.Exercise.Name))
            {
                throw new BusinessException("Упражнение с таким названием уже существует");
            }

            var exerciseDb = exercisesDb.FirstOrDefault(t => t.Id == param.Exercise.Id);
            if (exerciseDb == null && param.Exercise.Id != 0)
            {
                throw new BusinessException("У вас нет прав на редактирование данного упражнения");
            }

            //базовые упражнения (без UserId) редактируются только администратором
            bool isAdmin = await _userRoleCommands.IHaveRole(UserRoles.Admin);
            if (!isAdmin && exerciseDb != null && !(exerciseDb.UserId > 0))
            {
                throw new BusinessException("Базовый справочник упражнений редактируют только администраторы");
            }

            var targetExercise = _mapper.Map<ExerciseDb>(param.Exercise);
            if (exerciseDb != null)
            {
                targetExercise.Id = exerciseDb.Id;
                targetExercise.UserId = exerciseDb.UserId;

                _exerciseRepository.Update(targetExercise);
                return true;
            }

            targetExercise.Id = 0;
            targetExercise.UserId = _user.Id;
            await _exerciseRepository.CreateAsync(targetExercise);

            return true;
        }

        private void Validate(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new BusinessException("Укажите упражнение");
            }

            if (exercise.ExerciseTypeId == 0)
            {
                throw new BusinessException("Укажите тип упражнения");
            }

            if (exercise.ExerciseSubTypeId == 0)
            {
                throw new BusinessException("Укажите подтип упражнения");
            }

            if (string.IsNullOrEmpty(exercise.Name))
            {
                throw new BusinessException("Название обязательно для заполнения");
            }
        }

        public class Param
        {
            public Exercise Exercise { get; set; }
        }
    }
}
