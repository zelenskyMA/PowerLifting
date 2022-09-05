using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PowerLifting.Application;
using PowerLifting.Application.Mapper;
using PowerLifting.Application.TrainingPlan;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Repositories;
using PowerLifting.Infrastructure;
using PowerLifting.Infrastructure.Repositories;
using PowerLifting.Infrastructure.Repositories.TrainingPlan;

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
builder.Services.AddScoped<ICrudRepo<TrainingPlanDb>, TrainingPlanRepository>();
builder.Services.AddScoped<ITrainingDayRepository, TrainingDayRepository>();
builder.Services.AddScoped<IPlannedExerciseRepository, PlannedExerciseRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseSettingsRepository, ExerciseSettingsRepository>();
builder.Services.AddScoped<IPercentageRepository, PercentageRepository>();
builder.Services.AddScoped<ICrudRepo<ExercisePercentageDb>, ExercisePercentageRepository>();

builder.Services.AddScoped<ICrudRepo<DictionaryDb>, DictionaryRepository>();
builder.Services.AddScoped<ICrudRepo<DictionaryTypeDb>, DictionaryTypeRepository>();

//app services
builder.Services.AddScoped<ITrainingPlanCommands, TrainingPlanCommands>();
builder.Services.AddScoped<IPlannedExerciseCommands, PlannedExerciseCommands>();
builder.Services.AddScoped<IExerciseCommands, ExerciseCommands>();

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
