using AutoMapper;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class ExerciseCommands : IExerciseCommands
    {
        private readonly ICrudRepo<ExerciseDb> _exerciseRepository;
        private readonly IDictionaryCommands _dictionaryCommands;
        private readonly IMapper _mapper;

        public ExerciseCommands(
         ICrudRepo<ExerciseDb> exerciseRepository,
         IDictionaryCommands dictionaryCommands,
         IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _dictionaryCommands = dictionaryCommands;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<Exercise>> GetListAsync()
        {
            var dataDb = await _exerciseRepository.GetAllAsync();
            var exercises = dataDb.Select(t => _mapper.Map<Exercise>(t)).ToList();
            await SetDictionaryData(exercises);

            return exercises;
        }

        /// <inheritdoc />
        public async Task<List<Exercise>> GetAsync(List<int> ids)
        {
            var dataDb = await _exerciseRepository.FindAsync(t => ids.Contains(t.Id));
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
