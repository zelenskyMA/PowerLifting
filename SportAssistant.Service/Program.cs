using AutoMapper;
using LoggerLib.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SportAssistant.Application.Coaching.TrainingGroupCommands;
using SportAssistant.Application.Coaching.TrainingGroupUserCommands;
using SportAssistant.Application.Coaching.TrainingRequestCommands;
using SportAssistant.Application.Common;
using SportAssistant.Application.Common.Actions.EmailNotifications;
using SportAssistant.Application.Common.Actions.TrainingCounters;
using SportAssistant.Application.Dictionaryies;
using SportAssistant.Application.Management.CoachAssignment;
using SportAssistant.Application.Management.ManagerCommands;
using SportAssistant.Application.Management.OrganizationCommands;
using SportAssistant.Application.ReportGeneration;
using SportAssistant.Application.Settings;
using SportAssistant.Application.TrainingPlan.ExerciseCommands;
using SportAssistant.Application.TrainingPlan.PlanCommands;
using SportAssistant.Application.TrainingPlan.PlanDayCommands;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands;
using SportAssistant.Application.TrainingTemplate.TemplateDayCommands;
using SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands;
using SportAssistant.Application.TrainingTemplate.TemplateExerciseSettingsCommands;
using SportAssistant.Application.TrainingTemplate.TemplatePlanCommands;
using SportAssistant.Application.TraininTemplate.TemplateSetCommands;
using SportAssistant.Application.UserData;
using SportAssistant.Application.UserData.Auth;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Application.UserData.UserAchivementCommands;
using SportAssistant.Application.UserData.UserCommands;
using SportAssistant.Application.UserData.UserInfoCommands;
using SportAssistant.Domain.Interfaces;
using SportAssistant.Domain.Interfaces.Actions.EmailNotifications;
using SportAssistant.Domain.Interfaces.Coaching.Application;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Domain.Interfaces.Common;
using SportAssistant.Domain.Interfaces.Management;
using SportAssistant.Domain.Interfaces.ReportGeneration;
using SportAssistant.Domain.Interfaces.Settings.Application;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Interfaces.UserData.Application;
using SportAssistant.Infrastructure.Repositories.Coaching;
using SportAssistant.Infrastructure.Repositories.TrainingPlan;
using SportAssistant.Service.Middleware;
using System.Text;
using ConfigurationManager = SportAssistant.Service.Extensions.ConfigurationManager;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddConnectionProvider(builder.Configuration.GetConnectionString("ConnectionDb"));
       //  builder.Services.AddConnectionProvider(builder.Configuration.GetConnectionString("ProdCopyConnectionDb"));       

        builder.Services.AddSingleton(new MapperConfiguration(t => t.AddProfile(new MapperProfile())).CreateMapper());

        // TODO: ������ �������� ����� ��������� �����������
        // builder.RegisterLogger();

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
        builder.Services.AddScoped<IProcessPlanUserId, ProcessPlanUserId>();
        builder.Services.AddScoped<IProcessPlanDay, ProcessPlanDay>();
        builder.Services.AddScoped<IProcessPlanExercise, ProcessPlanExercise>();
        builder.Services.AddScoped<IProcessPlanExerciseSettings, ProcessPlanExerciseSettings>();
        builder.Services.AddScoped<IProcessExercise, ProcessExercise>();
        builder.Services.AddScoped<ITrainingCountersSetup, TrainingCountersSetup>();

        builder.Services.AddScoped<IProcessSetUserId, ProcessSetUserId>();
        builder.Services.AddScoped<IProcessTemplateSet, ProcessTemplateSet>();
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

        builder.Services.AddScoped<IProcessCoachAssignment, ProcessCoachAssignment>();
        builder.Services.AddScoped<IProcessManager, ProcessManager>();
        builder.Services.AddScoped<IProcessOrgData, ProcessOrgData>();
        
        builder.Services.AddScoped<IProcessEmail, ProcessEmail>();
        builder.Services.AddScoped<IResetPasswordEmailHandler, ResetPasswordEmailHandler>();

        builder.Services.AddScoped<IProcessDictionary, ProcessDictionary>();
        builder.Services.AddScoped<IProcessSettings, ProcessSettings>();
        builder.Services.AddScoped<IUserProvider, UserProvider>();

        builder.Services.AddScoped<IDataCollector, DataCollector>();
        builder.Services.AddScoped<IFileCreation, FileCreation>();
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
        app.UseMiddleware<LoggingMiddleware>();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}