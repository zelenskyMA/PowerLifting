using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportAssistant.Infrastructure.DataContext;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TestFramework.DataGeneration;
using TestFramework.Presets;

namespace TestFramework;

[ExcludeFromCodeCoverage]
public class ServiceTestFixture<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    private DbConnection _connection;

    /// <summary>Прединициализированные данные. Доступ к их ид в текущей БД.</summary>
    public DataPreset Data { get; set; }

    /// <summary>Набор действий, изменяющих данные БД хоста.</summary>
    public ActionsPreset Actions { get; set; }


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
            Data = new DataPreset(ctx);
            Actions = new ActionsPreset();
        });
    }

    /// <summary>
    /// Получение билдера моделей с тестовыми данными
    /// </summary>
    public FixtureBuilder GetBuilder() => new(new CustomPropertyBuilder());   

    protected override void Dispose(bool disposing)
    {
        _connection?.Close();
        _connection?.Dispose();

        base.Dispose(disposing);
    }
}
