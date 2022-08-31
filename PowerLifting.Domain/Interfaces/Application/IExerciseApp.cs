using PowerLifting.Domain.Models;

namespace PowerLifting.Domain.Interfaces.Application
{
  public interface IExerciseApp
  {
    Task<List<DictionaryItem>> GetTypesAsync();
  }
}
