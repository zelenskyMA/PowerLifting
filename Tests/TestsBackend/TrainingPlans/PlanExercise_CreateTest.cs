using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Domain.Models.Basic;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanExercise_CreateTest : BaseTest
{
    public PlanExercise_CreateTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_PlanExercise_NoItems_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        int dayCounter = 4;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var request = new PlanExerciseCreateCommand.Param() { DayId = planDayId, Exercises = new List<Exercise>() };

        //Act
        var response = Client.Post<bool>($"/planExercise", request);

        //Assert
        response.Should().BeFalse();
    }

    [Fact]
    public void Create_PlanExercise_EcxeedEx_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        int dayCounter = 4;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var example = GetExercises().First();
        var settings = Client.Get<AppSettings>("/appSettings");

        var exercises = new List<Exercise>();
        for (int i = 0; i <= settings.MaxExercises; i++)
        {
            exercises.Add(example);
        }

        var request = new PlanExerciseCreateCommand.Param() { DayId = planDayId, Exercises = exercises };

        //Act
        var response = Client.Post($"/planExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Лимит упражнений в день превышен*");
    }

    [Fact]
    public void Create_PlanExercise_ByOthers_Fail()
    {
        //Arrange
        int dayCounter = 4;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var request = new PlanExerciseCreateCommand.Param() { DayId = planDayId, Exercises = GetExercises() };

        Factory.Actions.AuthorizeUser(Client); // чужим инфа недоступна
        var testDay = Client.Get<PlanDay>($"/planDay/{planDayId}"); // проверяем, что нет упражнений
        testDay.Exercises.Should().BeEmpty();

        Factory.Actions.AuthorizeNoCoachUser(Client);

        //Act
        var response = Client.Post($"/planExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Create_PlanExercise_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        int dayCounter = 6;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var request = new PlanExerciseCreateCommand.Param() { DayId = planDayId, Exercises = GetExercises() };

        var testDay = Client.Get<PlanDay>($"/planDay/{planDayId}"); // проверяем, что нет упражнений
        testDay.Exercises.Should().BeEmpty();

        //Act
        var response = Client.Post<bool>($"/planExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(GetExercises().Count);
    }    

    [Fact]
    public void Create_PlanExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        int dayCounter = 5;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var request = new PlanExerciseCreateCommand.Param() { DayId = planDayId, Exercises = GetExercises() };

        var testDay = Client.Get<PlanDay>($"/planDay/{planDayId}"); // проверяем, что нет упражнений
        testDay.Exercises.Should().BeEmpty();

        //Act
        var response = Client.Post<bool>($"/planExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(GetExercises().Count);
    }

    [Fact]
    public void CreateAndRemove_PlanExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        int dayCounter = 1;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var request = new PlanExerciseCreateCommand.Param()
        {
            DayId = planDayId,
            Exercises = new List<Exercise>() { GetExercises().First() } // берем только одно
        };

        var testDay = Client.Get<PlanDay>($"/planDay/{planDayId}"); // проверяем, что есть 2 упражнения
        testDay.Exercises.Count.Should().Be(2);

        //Act
        var response = Client.Post<bool>($"/planExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(1); // было 2, стало 1.
    }

    private List<Exercise> GetExercises() => Factory.Data.PlanDays.First().Exercises.Select(t => t.Exercise).ToList();
}
