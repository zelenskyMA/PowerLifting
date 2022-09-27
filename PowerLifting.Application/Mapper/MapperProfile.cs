using AutoMapper;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Models;
using PowerLifting.Domain.Models.Coaching;
using PowerLifting.Domain.Models.TrainingPlan;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            PlanProfile();
            UserProfile();
            CoachProfile();

            CreateMap<DictionaryDb, DictionaryItem>().ReverseMap();
            CreateMap<DictionaryTypeDb, DictionaryType>().ReverseMap();
        }

        private void UserProfile()
        {
            CreateMap<UserDb, UserModel>().ReverseMap();
            CreateMap<UserInfoDb, UserInfo>().ReverseMap();
            CreateMap<UserAchivementDb, UserAchivement>().ReverseMap();
            CreateMap<UserRoleDb, UserRole>().ReverseMap();
            CreateMap<UserBlockHistoryDb, UserBlockHistory>().ReverseMap();
        }

        private void PlanProfile()
        {
            CreateMap<PlanDb, Plan>().ReverseMap();
            CreateMap<PlanDayDb, PlanDay>().ReverseMap();
            CreateMap<ExerciseDb, Exercise>().ReverseMap();
            CreateMap<PlanExerciseSettingsDb, PlanExerciseSettings>().ReverseMap();
            CreateMap<PercentageDb, Percentage>().ReverseMap();
            CreateMap<PlanExerciseDb, PlanExercise>()
                .ForPath(dest => dest.Exercise.Id, opt => opt.MapFrom(src => src.ExerciseId))
                .ReverseMap();
        }

        private void CoachProfile()
        {
            CreateMap<TrainingRequestDb, TrainingRequest>().ReverseMap();
            CreateMap<UserInfoDb, CoachInfo>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
        }
    }
}
