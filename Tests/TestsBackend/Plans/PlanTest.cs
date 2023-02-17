using FluentAssertions;
using SportAssistant.Domain.Models.UserData;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Plans;

public class PlanTest : BaseTest
{
    public PlanTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_Plan_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planId = Factory.Data.PlanDays[0].PlanId;

        //Act
        var plan = Client.Get<UserCard>($"/trainingPlan/get?id={planId}");

        //Assert
        plan.Should().NotBeNull();
        plan.UserId.Should().Be(Factory.Data.Users.First(t => t.Email == Constants.UserLogin).Id);
    }
}
