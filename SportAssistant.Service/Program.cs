using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SportAssistant.Application.Coaching.TrainingGroupCommands;
using SportAssistant.Application.Coaching.TrainingGroupUserCommands;
using SportAssistant.Application.Coaching.TrainingRequestCommands;
using SportAssistant.Application.Common;
using SportAssistant.Application.Common.Actions.TrainingCounters;
using SportAssistant.Application.Dictionaryies;
using SportAssistant.Application.Settings;
using SportAssistant.Application.TrainingPlan.ExerciseCommands;
using SportAssistant.Application.TrainingPlan.PlanCommands;
using SportAssistant.Application.TrainingPlan.PlanDayCommands;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands;
using SportAssistant.Application.TraininTemplate.TemplateDayCommands;
using SportAssistant.Application.TraininTemplate.TemplateExerciseCommands;
using SportAssistant.Application.TraininTemplate.TemplateExerciseSettingsCommands;
using SportAssistant.Application.TraininTemplate.TemplatePlanCommands;
using SportAssistant.Application.UserData;
using SportAssistant.Application.UserData.Auth;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Application.UserData.UserAchivementCommands;
using SportAssistant.Application.UserData.UserCommands;
using SportAssistant.Application.UserData.UserInfoCommands;
using SportAssistant.Domain.Interfaces;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Infrastructure.Repositories.Coaching;
using SportAssistant.Infrastructure.Repositories.TrainingPlan;
using SportAssistant.Service.Middleware;
using System.Text;
using ConfigurationManager = SportAssistant.Service.Extensions.ConfigurationManager;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddConnectionProvider(builder.Configuration.GetConnectionString("ConnectionDb"));
        builder.Services.AddSingleton(new MapperConfiguration(t => t.AddProfile(new MapperProfile())).CreateMapper());

        RegisterRepositories(builder);
        RegisterApps(builder);

        RegisterJwtAuth(builder);

        SetupApp(builder);
    }

    private static void RegisterRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddRepositoriesFromAssemblyOf<PlanRepository>();
        builder.Services.AddScoped<ITrainingRequestRepository, TrainingRequestRepository>();
        builder.Services.AddScoped<ITrainingGroupUserRepository, TrainingGroupUserRepository>();
    }

    private static void RegisterApps(WebApplicationBuilder builder)
    {
        builder.Services.AddCommandsFromAssemblyOf<UserInfoGetQuery>();

        builder.Services.AddScoped<IProcessPlan, ProcessPlan>();
        builder.Services.AddScoped<IProcessPlanDay, ProcessPlanDay>();
        builder.Services.AddScoped<IProcessPlanExercise, ProcessPlanExercise>();
        builder.Services.AddScoped<IProcessPlanExerciseSettings, ProcessPlanExerciseSettings>();
        builder.Services.AddScoped<IProcessExercise, ProcessExercise>();
        builder.Services.AddScoped<ITrainingCountersSetup, TrainingCountersSetup>();

        builder.Services.AddScoped<IProcessTemplatePlan, ProcessTemplatePlan>();
        builder.Services.AddScoped<IProcessTemplateDay, ProcessTemplateDay>();
        builder.Services.AddScoped<IProcessTemplateExercise, ProcessTemplateExercise>();
        builder.Services.AddScoped<IProcessTemplateExerciseSettings, ProcessTemplateExerciseSettings>();

        builder.Services.AddScoped<IProcessGroup, ProcessGroup>();
        builder.Services.AddScoped<IProcessGroupUser, ProcessGroupUser>();
        builder.Services.AddScoped<IProcessRequest, ProcessRequest>();
        builder.Services.AddScoped<IAllowedUserIds, AllowedUserIds>();

        builder.Services.AddScoped<IProcessUser, ProcessUser>();
        builder.Services.AddScoped<IProcessUserInfo, ProcessUserInfo>();
        builder.Services.AddScoped<IProcessUserAchivements, ProcessUserAchivements>();
        builder.Services.AddScoped<IUserRoleCommands, UserRoleCommands>();
        builder.Services.AddScoped<IUserBlockCommands, UserBlockCommands>();

        builder.Services.AddScoped<IProcessDictionary, ProcessDictionary>();
        builder.Services.AddScoped<IProcessSettings, ProcessSettings>();
        builder.Services.AddScoped<IUserProvider, UserProvider>();
    }

    private static void RegisterJwtAuth(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ConfigurationManager.AppSetting["JWT:Issuer"],
                        ValidAudience = ConfigurationManager.AppSetting["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]))
                    };
                });
    }

    private static void SetupApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.UseCustomExceptionHandler(builder.Environment);

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html"); ;

        app.Run();
    }
}