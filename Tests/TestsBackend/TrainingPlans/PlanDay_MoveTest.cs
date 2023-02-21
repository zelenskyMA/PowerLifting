using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanDayCommands;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanDay_MoveTest : BaseTest
{
    public PlanDay_MoveTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Move_PlanDay_NoParams_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDay = Factory.Data.PlanDays[1];

        // нет ид дня
        var request = new PlanDayMoveCommand.Param() { Id = 0, PlanId = planDay.PlanId.Value, TargetDate = planDay.ActivityDate.AddDays(1) };
        var response = Client.Post($"/planDay/move", request);
        response.ReadErrorMessage().Should().Match("Не задан один из обязательных параметров*");

        // нет ид плана
        request = new PlanDayMoveCommand.Param() { Id = planDay.Id, PlanId = 0, TargetDate = planDay.ActivityDate.AddDays(1) };
        response = Client.Post($"/planDay/move", request);
        response.ReadErrorMessage().Should().Match("Не задан один из обязательных параметров*");

        // нет целевой даты
        request = new PlanDayMoveCommand.Param() { Id = planDay.Id, PlanId = planDay.PlanId.Value, TargetDate = null };
        response = Client.Post($"/planDay/move", request);
        response.ReadErrorMessage().Should().Match("Не задан один из обязательных параметров*");
    }

    [Fact]
    public void Move_PlanDay_WrongParams_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDay = Factory.Data.PlanDays[1];

        // не найден ид дня
        var request = new PlanDayMoveCommand.Param() { Id = 100, PlanId = planDay.PlanId.Value, TargetDate = planDay.ActivityDate.AddDays(1) };
        var responseNoId = Client.Post<bool>($"/planDay/move", request);
        responseNoId.Should().BeFalse(); // по этому ид не нашли упражнений, потому не падаем. поиск первичен, чтобы не жестить с ошибками.

        // не найден ид плана
        request = new PlanDayMoveCommand.Param() { Id = planDay.Id, PlanId = 100, TargetDate = planDay.ActivityDate.AddDays(1) };
        var response = Client.Post($"/planDay/move", request);
        response.ReadErrorMessage().Should().Match("Редактируемый день не входит в указанный план или не существует*");

        // не найдена целевая дата в текущем плане
        request = new PlanDayMoveCommand.Param() { Id = planDay.Id, PlanId = planDay.PlanId.Value, TargetDate = planDay.ActivityDate.AddDays(100) };
        response = Client.Post($"/planDay/move", request);
        response.ReadErrorMessage().Should().Match("В вашем текущем тренировочном плане нет указанной даты*");
    }   

    [Fact]
    public void Move_PlanDay_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var planDay = Factory.Data.PlanDays[1];
        var request = new PlanDayMoveCommand.Param()
        {
            Id = planDay.Id,
            PlanId = planDay.PlanId.Value,
            TargetDate = planDay.ActivityDate.AddDays(1)
        };

        //Act - Assert
        var response = Client.Post($"/planDay/move", request);
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Move_PlanDay_NoExercises_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDay = Factory.Data.PlanDays[6];
        var request = new PlanDayMoveCommand.Param()
        {
            Id = planDay.Id,
            PlanId = planDay.PlanId.Value,
            TargetDate = planDay.ActivityDate.AddDays(1)
        };

        //Act - Assert
        var response = Client.Post<bool>($"/planDay/move", request);
        response.Should().BeFalse();
    }

    [Fact]
    public void Move_PlanDay_ByCoach_Success()
    {
        Factory.Actions.AuthorizeCoach(Client);
        MoveSuccessTest();
    }

    [Fact]
    public void Move_PlanDay_ByOwner_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        MoveSuccessTest();
    }

    private void MoveSuccessTest()
    {
        //Arrange
        var planDay = Factory.Data.PlanDays[1];
        var targetDayId = Factory.Data.PlanDays[2].Id;
        var request = new PlanDayMoveCommand.Param()
        {
            Id = planDay.Id,
            PlanId = planDay.PlanId.Value,
            TargetDate = planDay.ActivityDate.AddDays(1)
        };

        // в целевом дне пусто
        var checkTargetDay = Client.Get<PlanDay>($"/planDay/get?id={targetDayId}");
        checkTargetDay.Should().NotBeNull();
        checkTargetDay.Id.Should().Be(targetDayId);
        checkTargetDay.Exercises.Should().BeEmpty();

        //Act
        var response = Client.Post<bool>($"/planDay/move", request);

        //Assert
        response.Should().BeTrue();

        // в источнике упражнения удалены
        var sourceDay = Client.Get<PlanDay>($"/planDay/get?id={planDay.Id}");
        sourceDay.Should().NotBeNull();
        sourceDay.Id.Should().Be(planDay.Id);
        sourceDay.Exercises.Should().BeEmpty();

        // в целевой день упражнения добавлены
        var resultDay = Client.Get<PlanDay>($"/planDay/get?id={targetDayId}");
        resultDay.Should().NotBeNull();
        resultDay.Id.Should().Be(targetDayId);
        resultDay.Exercises.Should().HaveCount(2);

        // откат
        response = Client.Post<bool>($"/planDay/move", new PlanDayMoveCommand.Param()
        {
            Id = targetDayId,
            PlanId = planDay.PlanId.Value,
            TargetDate = planDay.ActivityDate
        });
        response.Should().BeTrue();
    }
}
