using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PowerLifting.Application.Coaching.TrainingGroupCommands;
using PowerLifting.Application.Coaching.TrainingGroupUserCommands;
using PowerLifting.Application.Coaching.TrainingRequestCommands;
using PowerLifting.Application.Common;
using PowerLifting.Application.Dictionaryies;
using PowerLifting.Application.TrainingPlan;
using PowerLifting.Application.TrainingPlan.ExerciseCommands;
using PowerLifting.Application.TrainingPlan.PlanDayCommands;
using PowerLifting.Application.TrainingPlan.PlanExerciseCommands;
using PowerLifting.Application.TrainingPlan.PlanExerciseSettingsCommands;
using PowerLifting.Application.UserData;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Application.UserData.UserAchivementCommands;
using PowerLifting.Application.UserData.UserCommands;
using PowerLifting.Application.UserData.UserInfoCommands;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.Common;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Infrastructure.Repositories.Coaching;
using PowerLifting.Infrastructure.Repositories.TrainingPlan;
using PowerLifting.Service.Middleware;
using System.Text;
using ConfigurationManager = PowerLifting.Service.Extensions.ConfigurationManager;

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
        builder.Services.AddScoped<IPlanExerciseSettingsRepository, PlanExerciseSettingsRepository>();
        builder.Services.AddScoped<ITrainingRequestRepository, TrainingRequestRepository>();
        builder.Services.AddScoped<ITrainingGroupUserRepository, TrainingGroupUserRepository>();
    }

    private static void RegisterApps(WebApplicationBuilder builder)
    {
        builder.Services.AddCommandsFromAssemblyOf<UserInfoGetQuery>();

        builder.Services.AddScoped<IProcessPlanDay, ProcessPlanDay>();
        builder.Services.AddScoped<IProcessUserInfo, ProcessUserInfo>();
        builder.Services.AddScoped<IProcessDictionary, ProcessDictionary>();
        builder.Services.AddScoped<IProcessPlanExercise, ProcessPlanExercise>();
        builder.Services.AddScoped<IProcessPlanExerciseSettings, ProcessPlanExerciseSettings>();
        builder.Services.AddScoped<IProcessExercise, ProcessExercise>();
        builder.Services.AddScoped<IPlanCountersSetup, PlanCountersSetup>();
        builder.Services.AddScoped<IProcessUserAchivements, ProcessUserAchivements>();
        builder.Services.AddScoped<IProcessUser, ProcessUser>();
        builder.Services.AddScoped<IProcessRequest, ProcessRequest>();
        builder.Services.AddScoped<IProcessGroup, ProcessGroup>();
        builder.Services.AddScoped<IProcessGroupUser, ProcessGroupUser>();
        
        builder.Services.AddScoped<IUserRoleCommands, UserRoleCommands>();
        builder.Services.AddScoped<IUserBlockCommands, UserBlockCommands>();

        builder.Services.AddScoped<IUserProvider, UserProvider>();
        builder.Services.AddScoped<IAllowedUserIds, AllowedUserIds>();        
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