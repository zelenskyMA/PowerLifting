using AutoMapper;
using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.Management;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models;
using SportAssistant.Domain.Models.Basic;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;
using SportAssistant.Domain.Models.UserData;

namespace SportAssistant.Application.Common;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        PlanProfile();
        UserProfile();
        CoachProfile();
        TemplateProfile();
        ManagementProfile();

        CreateMap<DictionaryDb, DictionaryItem>().ReverseMap();
        CreateMap<DictionaryTypeDb, DictionaryType>().ReverseMap();
        CreateMap<EmailMessageDb, EmailMessage>().ReverseMap();
    }

    private void UserProfile()
    {
        CreateMap<UserDb, UserModel>().ReverseMap();
        CreateMap<UserAchivementDb, UserAchivement>().ReverseMap();
        CreateMap<UserRoleDb, UserRole>().ReverseMap();
        CreateMap<UserBlockHistoryDb, UserBlockHistory>().ReverseMap();

        CreateMap<UserInfoDb, UserInfo>().ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
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
        CreateMap<TrainingGroupDb, TrainingGroup>().ReverseMap();

        CreateMap<UserInfoDb, CoachInfo>()
            .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();
    }

    private void TemplateProfile()
    {
        CreateMap<TemplateSetDb, TemplateSet>().ReverseMap();
        CreateMap<TemplatePlanDb, TemplatePlan>().ReverseMap();
        CreateMap<TemplateDayDb, TemplateDay>().ReverseMap();
        CreateMap<TemplateExerciseSettingsDb, TemplateExerciseSettings>().ReverseMap();
        CreateMap<TemplateExerciseDb, TemplateExercise>()
            .ForPath(dest => dest.Exercise.Id, opt => opt.MapFrom(src => src.ExerciseId))
            .ReverseMap();
    }

    private void ManagementProfile()
    {
        CreateMap<OrganizationDb, Organization>().ReverseMap();
        CreateMap<ManagerDb, Manager>().ReverseMap();        
    }
}
