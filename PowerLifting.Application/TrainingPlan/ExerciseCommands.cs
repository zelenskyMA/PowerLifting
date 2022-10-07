using AutoMapper;
using PowerLifting.Application.UserData;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class ExerciseCommands : IExerciseCommands
    {
        private readonly IUserRoleCommands _userRoleCommands;

        private readonly ICrudRepo<ExerciseDb> _exerciseRepository;
        private readonly IDictionaryCommands _dictionaryCommands;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public ExerciseCommands(
         IUserRoleCommands userRoleCommands,
         ICrudRepo<ExerciseDb> exerciseRepository,
         IDictionaryCommands dictionaryCommands,
         IUserProvider user,
         IMapper mapper)
        {
            _userRoleCommands = userRoleCommands;
            _exerciseRepository = exerciseRepository;
            _dictionaryCommands = dictionaryCommands;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Exercise?> GetAsync(int id) => (await GetAsync(new List<int>() { id })).FirstOrDefault();

        /// <inheritdoc />
        public async Task<List<Exercise>> GetAsync(List<int> ids)
        {
            var dataDb = await _exerciseRepository.FindAsync(t => ids.Contains(t.Id));
            var exercises = dataDb.Select(t => _mapper.Map<Exercise>(t)).ToList();
            await SetDictionaryData(exercises);

            return exercises;
        }

        /// <inheritdoc />
        public async Task<List<Exercise>> GetPlanningListAsync() => await GetAllowedExercises(new int?[] { null, 0, _user.Id });

        /// <inheritdoc />
        public async Task<List<Exercise>> GetEditingListAsync() => await GetAllowedExercises(new int?[] { _user.Id });

        /// <inheritdoc />
        public async Task<List<Exercise>> GetAdminEditingListAsync() => await GetAllowedExercises(new int?[] { null, 0 });

        /// <inheritdoc />
        public async Task UpdateAsync(Exercise exercise)
        {
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

            var allowedUserIds = new int?[] { null, 0, _user.Id };
            var exercisesDb = await _exerciseRepository.FindAsync(t => allowedUserIds.Contains(t.UserId)
                 && !t.Closed);

            if (exercisesDb.Any(t => t.Id != exercise.Id && t.Name == exercise.Name))
            {
                throw new BusinessException("Упражнение с таким названием уже существует");
            }

            var exerciseDb = exercisesDb.FirstOrDefault(t => t.Id == exercise.Id);
            if (exerciseDb == null && exercise.Id != 0)
            {
                throw new BusinessException("У вас нет прав на редактирование данного упражнения");
            }

            //базовые упражнения (без UserId) редактируются только администратором
            bool isAdmin = await _userRoleCommands.IHaveRole(UserRoles.Admin);
            if (!isAdmin && exerciseDb != null && !(exerciseDb.UserId > 0))
            {
                throw new BusinessException("Базовый справочник упражнений редактируют только администраторы");
            }

            var targetExercise = _mapper.Map<ExerciseDb>(exercise);
            if (exerciseDb != null)
            {
                exerciseDb.Name = targetExercise.Name;
                exerciseDb.Description = targetExercise.Description;
                exerciseDb.ExerciseTypeId = targetExercise.ExerciseTypeId;
                exerciseDb.ExerciseSubTypeId = targetExercise.ExerciseSubTypeId;

                await _exerciseRepository.UpdateAsync(exerciseDb);
                return;
            }
                        
            targetExercise.UserId = _user.Id;
            await _exerciseRepository.CreateAsync(targetExercise);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var allowedUserIds = new int?[] { null, 0, _user.Id };
            var exercisesDb = await _exerciseRepository.FindAsync(t => t.Id == id);

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
            await _exerciseRepository.UpdateAsync(exerciseDb);
        }

        private async Task<List<Exercise>> GetAllowedExercises(int?[] allowedUserIds)
        {
            var dataDb = await _exerciseRepository.FindAsync(t => allowedUserIds.Contains(t.UserId) && !t.Closed);
            var exercises = dataDb.Select(t => _mapper.Map<Exercise>(t)).ToList();
            await SetDictionaryData(exercises);

            return exercises;
        }

        private async Task SetDictionaryData(List<Exercise> exercises)
        {
            var ids = exercises.Select(t => t.ExerciseTypeId).Union(exercises.Select(t => t.ExerciseSubTypeId));
            var dictItems = await _dictionaryCommands.GetItemsAsync(ids.ToList());

            foreach (var item in exercises)
            {
                item.ExerciseTypeName = dictItems.First(t => t.Id == item.ExerciseTypeId).Name;
                item.ExerciseSubTypeName = dictItems.First(t => t.Id == item.ExerciseSubTypeId).Name;
            }
        }
    }
}
