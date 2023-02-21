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
        var planDayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get($"/planDay/get?id={planDayId}");

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_PlanDay_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act
        var response = Client.Get($"/planDay/get?id=-1");

        //Assert
        string respContentString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        respContentString.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Get_PlanDay_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var planDayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get($"/planDay/get?id={planDayId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_PlanDay_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planDayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get<PlanDay>($"/planDay/get?id={planDayId}");

        //Assert
        response.Should().NotBeNull();
        VerifyPlanCheck(response, planDayId);
    }

    [Fact]
    public void Get_PlanDay_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planDayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get<PlanDay>($"/planDay/get?id={planDayId}");

        //Assert
        response.Should().NotBeNull();
        VerifyPlanCheck(response, planDayId);
    }

    [Fact]
    public void Get_CurrentDay_None_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);

        //Act
        var response = Client.Get<PlanDay>($"/planDay/getCurrent");

        //Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(0);
        response.PlanId.Should().BeNull();
    }

    [Fact]
    public void Get_CurrentDay_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act
        var response = Client.Get<PlanDay>($"/planDay/getCurrent");

        //Assert
        response.Should().NotBeNull();
        response.ActivityDate.Should().Be(DateTime.Now.Date);
        VerifyPlanCheck(response, response.Id);
    }

    private void VerifyPlanCheck(PlanDay planDay, int? planDayId)
    {
        // тренировочный день плана
        planDay.Should().NotBeNull();
        planDay.Id.Should().BeGreaterThan(0);
        planDay.ActivityDate.Should().BeAfter(DateTime.Now.AddDays(-1));

        planDay.WeightLoadSum.Should().BeGreaterThan(0);
        planDay.IntensitySum.Should().BeGreaterThan(0);
        planDay.LiftCounterSum.Should().BeGreaterThan(0);

        planDay.ExerciseTypeCounters.Should().NotBeEmpty();
        planDay.ExerciseTypeCounters[0].Value.Should().BeGreaterThan(0);
        planDay.ExerciseTypeCounters[0].Name.Should().NotBeNullOrEmpty();

        // Упражнения в тренировочном дне        
        planDay.Exercises.Should().HaveCount(2);
        var dayExercise = planDay.Exercises[0];
        dayExercise.Id.Should().BeGreaterThan(0);
        dayExercise.Intensity.Should().BeGreaterThan(0);
        dayExercise.LiftCounter.Should().BeGreaterThan(0);
        dayExercise.WeightLoad.Should().BeGreaterThan(0);
        dayExercise.Order.Should().BeGreaterThan(0);

        dayExercise.SettingsTemplate.Should().NotBeNull(); // шаблон не назначен, но заглушка
        dayExercise.SettingsTemplate.Id.Should().Be(0);

        dayExercise.LiftIntensities.Should().NotBeEmpty();
        dayExercise.LiftIntensities[0].Percentage.Should().NotBeNull();

        // Упражнение из справочника, назначенное в план
        var exercise = dayExercise.Exercise;
        exercise.Should().NotBeNull();
        exercise.Id.Should().BeGreaterThan(0);
        exercise.ExerciseTypeId.Should().BeGreaterThan(0);
        exercise.ExerciseSubTypeId.Should().BeGreaterThan(0);

        // Настройки поднятий в упражнении
        dayExercise.Settings.Should().NotBeEmpty();
        var settings = dayExercise.Settings[0];
        settings.Id.Should().BeGreaterThan(0);
        settings.Completed.Should().BeFalse();
        settings.Weight.Should().BeGreaterThan(0);
        settings.Iterations.Should().BeGreaterThan(0);
        settings.ExercisePart1.Should().BeGreaterThan(0);
        settings.ExercisePart2.Should().BeGreaterThan(0);
        settings.ExercisePart3.Should().BeGreaterThan(0);
        settings.PlanExerciseId.Should().BeGreaterThan(0);

        // процентовка поднятия
        settings.Percentage.Should().NotBeNull();
        settings.Percentage.Id.Should().BeGreaterThan(0);
        settings.Percentage.MaxValue.Should().BeGreaterThan(0);
        settings.Percentage.MinValue.Should().BeGreaterThan(0);
        settings.Percentage.Name.Should().NotBeNullOrEmpty();
        settings.Percentage.Description.Should().BeNullOrEmpty();
    }
}
