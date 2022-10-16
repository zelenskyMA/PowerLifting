using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan.ExerciseCommands
{
    public class ProcessExercise : IProcessExercise
    {
        private readonly ICrudRepo<ExerciseDb> _exerciseRepository;
        private readonly IProcessDictionary _processDictionary;
        private readonly IMapper _mapper;

        public ProcessExercise(
         ICrudRepo<ExerciseDb> exerciseRepository,
         IProcessDictionary dictionaryCommands,
         IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _processDictionary = dictionaryCommands;
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

        public async Task<List<Exercise>> GetAllowedExercises(int?[] allowedUserIds)
        {
            var dataDb = await _exerciseRepository.FindAsync(t => allowedUserIds.Contains(t.UserId) && !t.Closed);
            var exercises = dataDb.Select(t => _mapper.Map<Exercise>(t)).ToList();
            await SetDictionaryData(exercises);

            return exercises;
        }

        public async Task SetDictionaryData(List<Exercise> exercises)
        {
            var ids = exercises.Select(t => t.ExerciseTypeId).Union(exercises.Select(t => t.ExerciseSubTypeId));
            var dictItems = await _processDictionary.GetItemsAsync(ids.ToList());

            foreach (var item in exercises)
            {
                item.ExerciseTypeName = dictItems.First(t => t.Id == item.ExerciseTypeId).Name;
                item.ExerciseSubTypeName = dictItems.First(t => t.Id == item.ExerciseSubTypeId).Name;
            }
        }
    }
}
