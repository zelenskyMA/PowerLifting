using AutoMapper;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Models;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TrainingPlanDb, TrainingPlanModel>().ReverseMap();
            CreateMap<TrainingDayDb, TrainingDay>().ReverseMap();
            CreateMap<ExerciseDb, Exercise>().ReverseMap();

            CreateMap<DictionaryDb, DictionaryItem>().ReverseMap();
            CreateMap<DictionaryTypeDb, DictionaryType>().ReverseMap();
        }
    }
}
