using AutoMapper;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Models;
using PowerLifting.Domain.Models.TrainingWork;

namespace PowerLifting.Application.Mapper
{
  public class MapperProfile : Profile
  {
    public MapperProfile()
    {
      CreateMap<TrainingPlanDb, TrainingPlan>().ReverseMap();
      CreateMap<TrainingDayDb, TrainingDay>().ReverseMap();

      CreateMap<ExerciseTypeDb, DictionaryItem>().ReverseMap();
      CreateMap<ExerciseDb, Exercise>().ReverseMap();
    }
  }
}
