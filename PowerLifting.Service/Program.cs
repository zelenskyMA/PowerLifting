using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PowerLifting.Application;
using PowerLifting.Application.Mapper;
using PowerLifting.Domain.Interfaces.Application;
using PowerLifting.Domain.Interfaces.Repositories;
using PowerLifting.Infrastructure;
using PowerLifting.Infrastructure.Repositories;

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
builder.Services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
builder.Services.AddScoped<ITrainingDayRepository, TrainingDayRepository>();

//app services
builder.Services.AddScoped<ITrainingPlanApp, TrainingPlanApp>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
