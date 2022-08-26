using Microsoft.AspNetCore.Mvc;
using PowerLifting.Service;

namespace PowerLifting.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    [HttpGet]
    [Route("workday")]
    public WorkDay GetWorkDay()
    {
      var ss = new WorkDay
      {
        Name = "жим лежа",
        ExerciseCount = 10,
        Data = new List<ExerciseData>()
      };

      for (int i = 0; i < 9; i++)
      {
        ss.Data.Add(new ExerciseData() { IterationCount = i, Weight = i * 4, RepeateCount1 = i + 1, RepeateCount2 = i + 2, RepeateCount3 = i + 3 });
      }

      return ss;
    }
  }
}