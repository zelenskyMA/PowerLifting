using FluentAssertions;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanDay_ClearTest : BaseTest
{
    public PlanDay_ClearTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Clear_PlanDay_WrongId_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Delete<bool>($"/planDay/0");
        response.Should().BeTrue();
    }

    [Fact]
    public void Clear_PlanDay_NoExercises_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDayId = Factory.Data.PlanDays[6].Id;

        //Act - Assert
        var response = Client.Delete<bool>($"/planDay/{planDayId}");
        response.Should().BeTrue();
    }

    [Fact]
    public void Clear_PlanDay_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var planDayId = Factory.Data.PlanDays[1].Id;

        //Act - Assert
        var response = Client.Delete($"/planDay/{planDayId}");
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Move_PlanDay_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planDayId = Factory.Data.PlanDays[0].Id; // удаляем 1-ый день без отката

        // в целевом дне НЕ пусто
        var checkTargetDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        checkTargetDay.Should().NotBeNull();
        checkTargetDay.Id.Should().Be(planDayId);
        checkTargetDay.Exercises.Should().NotBeEmpty();

        //Act
        var response = Client.Delete<bool>($"/planDay/{planDayId}");

        //Assert
        response.Should().BeTrue();

        // упражнения удалены
        var sourceDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        sourceDay.Should().NotBeNull();
        sourceDay.Id.Should().Be(planDayId);
        sourceDay.Exercises.Should().BeEmpty();
    }

    [Fact]
    public void Move_PlanDay_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDayId = Factory.Data.PlanDays[1].Id; // удаляем 2-ой день без отката

        // в целевом дне НЕ пусто
        var checkTargetDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        checkTargetDay.Should().NotBeNull();
        checkTargetDay.Id.Should().Be(planDayId);
        checkTargetDay.Exercises.Should().NotBeEmpty();

        //Act
        var response = Client.Delete<bool>($"/planDay/{planDayId}");

        //Assert
        response.Should().BeTrue();

        // упражнения удалены
        var sourceDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        sourceDay.Should().NotBeNull();
        sourceDay.Id.Should().Be(planDayId);
        sourceDay.Exercises.Should().BeEmpty();
    }
}
