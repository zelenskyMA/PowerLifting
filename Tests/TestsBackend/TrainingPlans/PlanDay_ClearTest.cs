using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanDayCommands;
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
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var request = new PlanDayClearCommand.Param() { Id = 0 };

        //Act - Assert
        var response = Client.Post<bool>($"/planDay/clear", request);
        response.Should().BeTrue();
    }

    [Fact]
    public void Clear_PlanDay_NoExercises_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDay = Factory.Data.PlanDays[6];
        var request = new PlanDayClearCommand.Param() { Id = planDay.Id };

        //Act - Assert
        var response = Client.Post<bool>($"/planDay/clear", request);
        response.Should().BeTrue();
    }

    [Fact]
    public void Clear_PlanDay_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var planDay = Factory.Data.PlanDays[1];
        var request = new PlanDayClearCommand.Param() { Id = planDay.Id };

        //Act - Assert
        var response = Client.Post($"/planDay/clear", request);
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Move_PlanDay_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planDay = Factory.Data.PlanDays[0]; // удаляем 1-ый день без отката
        var request = new PlanDayClearCommand.Param() { Id = planDay.Id };

        // в целевом дне НЕ пусто
        var checkTargetDay = Client.Get<PlanDay>($"/planDay/get?id={planDay.Id}");
        checkTargetDay.Should().NotBeNull();
        checkTargetDay.Id.Should().Be(planDay.Id);
        checkTargetDay.Exercises.Should().NotBeEmpty();

        //Act
        var response = Client.Post<bool>($"/planDay/clear", request);

        //Assert
        response.Should().BeTrue();

        // упражнения удалены
        var sourceDay = Client.Get<PlanDay>($"/planDay/get?id={planDay.Id}");
        sourceDay.Should().NotBeNull();
        sourceDay.Id.Should().Be(planDay.Id);
        sourceDay.Exercises.Should().BeEmpty();
    }

    [Fact]
    public void Move_PlanDay_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDay = Factory.Data.PlanDays[1]; // удаляем 2-ой день без отката
        var request = new PlanDayClearCommand.Param() { Id = planDay.Id };

        // в целевом дне НЕ пусто
        var checkTargetDay = Client.Get<PlanDay>($"/planDay/get?id={planDay.Id}");
        checkTargetDay.Should().NotBeNull();
        checkTargetDay.Id.Should().Be(planDay.Id);
        checkTargetDay.Exercises.Should().NotBeEmpty();

        //Act
        var response = Client.Post<bool>($"/planDay/clear", request);

        //Assert
        response.Should().BeTrue();

        // упражнения удалены
        var sourceDay = Client.Get<PlanDay>($"/planDay/get?id={planDay.Id}");
        sourceDay.Should().NotBeNull();
        sourceDay.Id.Should().Be(planDay.Id);
        sourceDay.Exercises.Should().BeEmpty();
    }
}
