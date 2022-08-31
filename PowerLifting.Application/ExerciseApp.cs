using AutoMapper;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Application
{
  public class ExerciseApp : IExerciseApp
  {
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IMapper _mapper;

    public ExerciseApp(
     IExerciseRepository exerciseRepository,
     IMapper mapper)
    {
      _exerciseRepository = exerciseRepository;
      _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<List<Exercise>> GetListAsync()
    {
      var exercises = await _exerciseRepository.GetAllAsync();
      var exerciseTypes = await _exerciseRepository.GetExerciseTypesAsync();
      
      var result = exercises.Select(t => _mapper.Map<Exercise>(t)).ToList();
      foreach (var item in result)
      {
        item.ExerciseTypeName = exerciseTypes.First(t=> t.Id == item.ExerciseTypeId).Name;
      }

      return result;
    }
  }
}
