using AutoMapper;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class ExerciseCommands : IExerciseCommands
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IDictionaryCommands _dictionaryCommands;
        private readonly IMapper _mapper;

        public ExerciseCommands(
         IExerciseRepository exerciseRepository,
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
            var exercises = await _exerciseRepository.GetAllAsync();
            var result = exercises.Select(t => _mapper.Map<Exercise>(t)).ToList();

            var ids = exercises.Select(t => t.ExerciseTypeId).Union(exercises.Select(t => t.ExerciseSubTypeId));
            var dictItems = await _dictionaryCommands.GetItemsAsync(ids.ToList());
            foreach (var item in result)
            {
                item.ExerciseTypeName = dictItems.First(t => t.Id == item.ExerciseTypeId).Name;
                item.ExerciseSubTypeName = dictItems.First(t => t.Id == item.ExerciseSubTypeId).Name;
            }

            return result;
        }
    }
}
