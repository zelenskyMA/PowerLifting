using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PowerLifting.Application;
using PowerLifting.Application.Administration;
using PowerLifting.Application.Analitics;
using PowerLifting.Application.Coaching;
using PowerLifting.Application.Mapper;
using PowerLifting.Application.TrainingPlan;
using PowerLifting.Application.TrainingPlan.Process;
using PowerLifting.Application.UserData;
using PowerLifting.Application.UserData.Auth;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Administration;
using PowerLifting.Domain.Interfaces.Analitics.Application;
using PowerLifting.Domain.Interfaces.Coaching.Application;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application.Process;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Infrastructure;
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

        var connString = builder.Configuration.GetConnectionString("ConnectionDb");
        builder.Services.AddDbContext<LiftingContext>(options => options.UseSqlServer(connString));

        var mapperConfig = new MapperConfiguration(t => t.AddProfile(new MapperProfile()));
        builder.Services.AddSingleton(mapperConfig.CreateMapper());

        builder.Services.AddScoped<IUserProvider, UserProvider>();

        RegisterRepositories(builder);
        RegisterCommands(builder);

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

        SetupApp(builder);
    }

    private static void RegisterRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddRepositoriesFromAssemblyOf<PlanRepository>();
        builder.Services.AddScoped<IPlanExerciseSettingsRepository, PlanExerciseSettingsRepository>();
        builder.Services.AddScoped<ITrainingRequestRepository, TrainingRequestRepository>();
        builder.Services.AddScoped<IUserTrainingGroupRepository, UserTrainingGroupRepository>();
    }

    private static void RegisterCommands(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPlanCommands, PlanCommands>();
        builder.Services.AddScoped<IPlanExerciseCommands, PlanExerciseCommands>();
        builder.Services.AddScoped<IPlanExerciseSettingsCommands, PlanExerciseSettingsCommands>();
        builder.Services.AddScoped<IExerciseCommands, ExerciseCommands>();
        builder.Services.AddScoped<IPlanCountersSetup, PlanCountersSetup>();

        builder.Services.AddScoped<IUserAchivementCommands, UserAchivementCommands>();
        builder.Services.AddScoped<IUserCommands, UserCommands>();
        builder.Services.AddScoped<IUserInfoCommands, UserInfoCommands>();
        builder.Services.AddScoped<IUserRoleCommands, UserRoleCommands>();
        builder.Services.AddScoped<IUserBlockCommands, UserBlockCommands>();

        builder.Services.AddScoped<IUserAdministrationCommands, UserAdministrationCommands>();

        builder.Services.AddScoped<ITrainingRequestCommands, TrainingRequestCommands>();
        builder.Services.AddScoped<ITrainingGroupCommands, TrainingGroupCommands>();
        builder.Services.AddScoped<IUserTrainingGroupCommands, UserTrainingGroupCommands>();

        builder.Services.AddScoped<IPlanAnaliticsCommands, PlanAnaliticsCommands>();

        builder.Services.AddScoped<IDictionaryCommands, DictionaryCommands>();
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