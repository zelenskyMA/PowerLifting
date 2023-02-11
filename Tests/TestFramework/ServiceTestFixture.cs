using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportAssistant.Application.UserData.UserCommands.UserCommands;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.UserData.Auth;
using SportAssistant.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// <summary> Предсозданные пользователи для использования в тестах</summary>
    public List<UserDb> Users { get; }

    /// <summary> Предсозданный план для тестов</summary>
    public PlanDay PlanDay { get; set; }

    private DbConnection _connection;

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
            // create new Db with new connection
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // replace old db with new one in DI
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SportContext>));
            services.Remove(descriptor);
            services.AddDbContext<SportContext>(options => options.UseSqlite(_connection));

            // get context
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var ctx = scopedServices.GetRequiredService<SportContext>();

            // seed context with data
            ctx.Database.EnsureCreated();
            DbSeed.InitializeDbForTests(ctx, Users);
            PlanDay = DbSeed.CreatePlan(ctx, 3, DateTime.Now);
        });
    }

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

    protected override void Dispose(bool disposing)
    {
        _connection?.Close();
        _connection?.Dispose();

        base.Dispose(disposing);
    }
}
