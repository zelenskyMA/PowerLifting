using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanExerciseSettingsCommands;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanExercise_CompleteTest : BaseTest
{
    public PlanExercise_CompleteTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Complete_PlanExercise_WrongEx_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var request = new PlanExerciseSettingsComplete.Param() { Ids = new List<int> { -1 } };

        //Act
        var response = Client.Post<bool>($"/planExercise/complete", request);

        //Assert
        response.Should().BeFalse();
    }

    [Fact]
    public void Complete_PlanExercise_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var settingId = Factory.Data.PlanDays[0].Exercises[0].Settings[0].Id;
        var request = new PlanExerciseSettingsComplete.Param() { Ids = new List<int> { settingId } };

        Factory.Data.PlanDays[0].Exercises[1].Settings[0].Completed.Should().BeFalse(); // открыто

        //Act
        var response = Client.Post($"/planExercise/complete", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Complete_PlanExercise_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planExId = Factory.Data.PlanDays[0].Exercises[1].Id;
        var settingId = Factory.Data.PlanDays[0].Exercises[1].Settings[0].Id;
        var request = new PlanExerciseSettingsComplete.Param() { Ids = new List<int> { settingId } };

        Factory.Data.PlanDays[0].Exercises[1].Settings[0].Completed.Should().BeFalse(); // открыто

        //Act
        var response = Client.Post<bool>($"/planExercise/complete", request);

        //Assert
        response.Should().BeTrue();

        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");
        exercise.Should().NotBeNull();
        exercise.Settings.First(t => t.Id == settingId).Completed.Should().BeTrue(); // закрыто
    }

    [Fact]
    public void Complete_PlanExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planExId = Factory.Data.PlanDays[0].Exercises[1].Id;
        var settingId = Factory.Data.PlanDays[0].Exercises[1].Settings[1].Id;
        var request = new PlanExerciseSettingsComplete.Param() { Ids = new List<int> { settingId } };

        Factory.Data.PlanDays[0].Exercises[1].Settings[0].Completed.Should().BeFalse(); // открыто

        //Act
        var response = Client.Post<bool>($"/planExercise/complete", request);

        //Assert
        response.Should().BeTrue();

        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");
        exercise.Should().NotBeNull();
        exercise.Settings.First(t => t.Id == settingId).Completed.Should().BeTrue(); // закрыто
    }
}
