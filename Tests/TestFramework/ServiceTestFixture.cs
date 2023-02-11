using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportAssistant.Application.UserData.UserCommands.UserCommands;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.UserData.Auth;
using SportAssistant.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using TestFramework;
using TestFramework.DataGeneration;
using TestFramework.TestExtensions;

namespace RazorPagesProject.Tests;

[ExcludeFromCodeCoverage]
public class ServiceTestFixture<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    public List<UserDb> Users { get; }

    public PlanDay PlanDay { get; set; }

    public ServiceTestFixture()
    {
        Users = new List<UserDb>() {
            new UserDb() { Id = 1, Email = Constants.AdminLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 2, Email = Constants.CoachLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 3, Email = Constants.UserLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 4, Email = Constants.BlockedUserLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, Blocked = true },
            new UserDb() { Id = 5, Email = Constants.User2Login, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
        };
    }

    /// <summary>
    /// Формирование хоста с подмененной БД
    /// </summary>
    /// <param name="builder"></param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SportContext>));
            services.Remove(descriptor);

            services.AddDbContext<SportContext>(options =>
            {
                options.UseInMemoryDatabase($"InMemoryDatabase");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var ctx = scopedServices.GetRequiredService<SportContext>();
            var logger = scopedServices.GetRequiredService<ILogger<ServiceTestFixture<TStartup>>>();

            ctx.Database.EnsureCreated();

            try
            {
                DbSeed.InitializeDbForTests(ctx, Users);
                PlanDay = DbSeed.CreatePlan(ctx, 3, DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred on database seeding for tests. Msg: {ex.Message}");
            }
        });
    }

    public void Init(SportContext ctx) => DbSeed.InitializeDbForTests(ctx, Users);

    /// <summary>
    /// Получение билдера моделей с тестовыми данными
    /// </summary>
    /// <returns></returns>
    public FixtureBuilder GetBuilder() => new(new CustomPropertyBuilder());

    public void AuthorizeAdmin(HttpClient client) => Authorize(client, Constants.AdminLogin);
    public void AuthorizeCoach(HttpClient client) => Authorize(client, Constants.CoachLogin);
    public void AuthorizeUser(HttpClient client) => Authorize(client, Constants.UserLogin);
    public void UnAuthorize(HttpClient client) => client.DefaultRequestHeaders.Remove("Authorization");

    private void Authorize(HttpClient client, string login)
    {
        var response = client.Post<TokenModel>("/user/login", new UserLoginCommand.Param() { Login = login, Password = Constants.Password });

        UnAuthorize(client);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.Token}");
    }
}
