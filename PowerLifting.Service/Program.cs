using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PowerLifting.Application;
using PowerLifting.Application.Mapper;
using PowerLifting.Application.TrainingPlan;
using PowerLifting.Application.UserData;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Infrastructure;
using PowerLifting.Infrastructure.Repositories;
using PowerLifting.Infrastructure.Repositories.TrainingPlan;
using PowerLifting.Infrastructure.Repositories.UserData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//misc services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("ConnectionDb");
builder.Services.AddDbContext<LiftingContext>(options => options.UseSqlServer(connString));

var mapperConfig = new MapperConfiguration(t => t.AddProfile(new MapperProfile()));
builder.Services.AddSingleton(mapperConfig.CreateMapper());

//repo services
builder.Services.AddScoped<ICrudRepo<PlanDb>, PlanRepository>();
builder.Services.AddScoped<ICrudRepo<PlanDayDb>, PlanDayRepository>();
builder.Services.AddScoped<ICrudRepo<PlanExerciseDb>, PlanExerciseRepository>();
builder.Services.AddScoped<IPlanExerciseSettingsRepository, PlanExerciseSettingsRepository>();
builder.Services.AddScoped<ICrudRepo<ExerciseDb>, ExerciseRepository>();

builder.Services.AddScoped<ICrudRepo<DictionaryDb>, DictionaryRepository>();
builder.Services.AddScoped<ICrudRepo<DictionaryTypeDb>, DictionaryTypeRepository>();

builder.Services.AddScoped<ICrudRepo<UserAchivementDb>, UserAchivementRepository>();
builder.Services.AddScoped<ICrudRepo<UserDb>, UserRepository>();

//app services
builder.Services.AddScoped<IPlanCommands, PlanCommands>();
builder.Services.AddScoped<IPlanExerciseCommands, PlanExerciseCommands>();
builder.Services.AddScoped<IPlanExerciseSettingsCommands, PlanExerciseSettingsCommands>();
builder.Services.AddScoped<IExerciseCommands, ExerciseCommands>();

builder.Services.AddScoped<IUserAchivementCommands, UserAchivementCommands>();

builder.Services.AddScoped<IDictionaryCommands, DictionaryCommands>();


var app = builder.Build();

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
