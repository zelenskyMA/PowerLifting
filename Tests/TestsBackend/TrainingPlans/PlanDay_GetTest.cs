using FluentAssertions;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanDay_GetTest : BaseTest
{
    public PlanDay_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_PlanDay_UnAuthorized_Fail()
    {
        //Arrange
        Factory.Actions.UnAuthorize(Client);
        var planDayId = Factory.Data.PlanDays.First();

        //Act
        var response = Client.Get($"/planDay/get?id={planDayId}");

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
