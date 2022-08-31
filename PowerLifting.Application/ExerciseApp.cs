using AutoMapper;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Domain.Models;

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

    public async Task<List<DictionaryItem>> GetTypesAsync()
    {
      var result = await _exerciseRepository.GetExerciseTypesAsync();
      return result.Select(t => _mapper.Map<DictionaryItem>(t)).ToList();
    }
  }
}
