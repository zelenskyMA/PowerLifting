using AutoFixture;
using FluentAssertions;
using SportAssistant.Domain.Models.Analitics;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace AppUser;

public class User_AnaliticsTest : BaseTest
{
    public User_AnaliticsTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void User_Analitics_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var startDate = ToIso(DateTime.Now.AddDays(-5));
        var finishDate = ToIso(DateTime.Now.AddDays(1));

        var response = Client.Get($"/analitics/getPlanAnalitics?startDate={startDate}&finishDate={finishDate}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void User_Analitics_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var startDate = ToIso(DateTime.Now.AddDays(-10));
        var finishDate = ToIso(DateTime.Now.AddDays(10));

        //Act
        var response = Client.Get($"/analitics/getPlanAnalitics/{userId}?startDate={startDate}&finishDate={finishDate}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void User_Analitics_ByOwner_Nothing_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var startDate = ToIso(DateTime.Now.AddDays(10));
        var finishDate = ToIso(DateTime.Now.AddDays(20));

        var response = Client.Get<PlanAnalitics>($"/analitics/getPlanAnalitics?startDate={startDate}&finishDate={finishDate}");
        response.TypeCounters.Should().HaveCount(0);
        response.PlanCounters.Should().HaveCount(0);
        response.FullTypeCounterList.Should().HaveCount(0);
    }

    [Fact]
    public void User_Analitics_ByOwner_WithData_Success()
    {
        Factory.Actions.AuthorizeUser(Client);

        // 2 плана
        var startDate = ToIso(DateTime.Now.AddDays(-10));
        var finishDate = ToIso(DateTime.Now.AddDays(10));

        var response = Client.Get<PlanAnalitics>($"/analitics/getPlanAnalitics?startDate={startDate}&finishDate={finishDate}");
        response.TypeCounters.Should().HaveCount(4);
        response.PlanCounters.Should().HaveCount(2);
        response.FullTypeCounterList.Should().HaveCount(2);

        var plan = response.PlanCounters[0]; // проверяем, что в подсчете учавствуют только завершенные поднятия
        plan.IntensitySum.Should().Be(80);
        plan.LiftCounterSum.Should().Be(12);
        plan.WeightLoadSum.Should().Be(960);

        // 1 плана
        startDate = ToIso(DateTime.Now);
        finishDate = ToIso(DateTime.Now.AddDays(10));

        response = Client.Get<PlanAnalitics>($"/analitics/getPlanAnalitics?startDate={startDate}&finishDate={finishDate}");
        response.TypeCounters.Should().HaveCount(4);
        response.PlanCounters.Should().HaveCount(1);
        response.FullTypeCounterList.Should().HaveCount(1);
    }

    [Fact]
    public void User_Analitics_ByCoach_Success()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);

        // 2 плана
        var startDate = ToIso(DateTime.Now.AddDays(-10));
        var finishDate = ToIso(DateTime.Now.AddDays(10));

        var response = Client.Get<PlanAnalitics>($"/analitics/getPlanAnalitics/{userId}?startDate={startDate}&finishDate={finishDate}");
        response.TypeCounters.Should().HaveCount(4);
        response.PlanCounters.Should().HaveCount(2);
        response.FullTypeCounterList.Should().HaveCount(2);

        // 1 плана
        startDate = ToIso(DateTime.Now);
        finishDate = ToIso(DateTime.Now.AddDays(10));

        response = Client.Get<PlanAnalitics>($"/analitics/getPlanAnalitics/{userId}?startDate={startDate}&finishDate={finishDate}");
        response.TypeCounters.Should().HaveCount(4);
        response.PlanCounters.Should().HaveCount(1);
        response.FullTypeCounterList.Should().HaveCount(1);
    }


    private string ToIso(DateTime date) => date.ToUniversalTime().ToString("u").Replace(" ", "T");
}
