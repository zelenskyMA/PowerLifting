using FluentAssertions;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanExercise_GetTest : BaseTest
{
    public PlanExercise_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_PlanExercise_UnAuthorized_Fail()
    {
        //Arrange
        Factory.Actions.UnAuthorize(Client);
        var planExId = Factory.Data.PlanDays.First().Exercises.First();

        //Act
        var response = Client.Get($"/planExercise/{planExId}");

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_PlanExercise_WrongId_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<PlanExercise>($"/planExercise/-1");
        response.Should().NotBeNull(); // заглушка
        response.Id.Should().Be(0);
    }

    [Fact]
    public void Get_PlanExercise_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var planExId = Factory.Data.PlanDays.First().Exercises.First().Id;

        //Act
        var response = Client.Get($"/planExercise/{planExId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_PlanExercise_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planExId = Factory.Data.PlanDays.First().Exercises.First().Id;

        //Act
        var response = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        //Assert
        response.Should().NotBeNull();
        VerifyPlanExerciseCheck(response, planExId);
    }

    [Fact]
    public void Get_PlanExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planExId = Factory.Data.PlanDays.First().Exercises.First().Id;

        //Act
        var response = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        //Assert
        response.Should().NotBeNull();
        VerifyPlanExerciseCheck(response, planExId);
    }

    [Fact]
    public void GetByDay_PlanExercise_WrongId_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<List<PlanExercise>>($"/planExercise/getByDay/-1");
        response.Should().BeEmpty();
    }

    [Fact]
    public void GetByDay_PlanExercise_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var dayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get($"/planExercise/getByDay/{dayId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void GetByDay_PlanExercise_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var dayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get<List<PlanExercise>>($"/planExercise/getByDay/{dayId}");

        //Assert
        response.Should().NotBeNull();
        response.Count().Should().Be(2);
        VerifyPlanExerciseCheck(response[0], response[0].Id);
    }

    [Fact]
    public void GetByDay_PlanExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var dayId = Factory.Data.PlanDays.First().Id;

        //Act
        var response = Client.Get<List<PlanExercise>>($"/planExercise/getByDay/{dayId}");

        //Assert
        response.Should().NotBeNull();
        response.Count().Should().Be(2);
        VerifyPlanExerciseCheck(response[0], response[0].Id);
    }


    private void VerifyPlanExerciseCheck(PlanExercise planExercise, int? planExId)
    {
        // Упражнения в тренировочном дне        
        planExercise.Id.Should().Be(planExId);
        planExercise.Intensity.Should().BeGreaterThan(0);
        planExercise.LiftCounter.Should().BeGreaterThan(0);
        planExercise.WeightLoad.Should().BeGreaterThan(0);
        planExercise.Order.Should().BeGreaterThan(0);

        planExercise.SettingsTemplate.Should().NotBeNull(); // шаблон не назначен, но заглушка
        planExercise.SettingsTemplate.Id.Should().Be(0);

        planExercise.LiftIntensities.Should().NotBeEmpty();
        planExercise.LiftIntensities[0].Percentage.Should().NotBeNull();

        // Упражнение из справочника, назначенное в план
        var exercise = planExercise.Exercise;
        exercise.Should().NotBeNull();
        exercise.Id.Should().BeGreaterThan(0);
        exercise.ExerciseTypeId.Should().BeGreaterThan(0);
        exercise.ExerciseSubTypeId.Should().BeGreaterThan(0);

        // Настройки поднятий в упражнении
        planExercise.Settings.Should().NotBeEmpty();
        var settings = planExercise.Settings[0];
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
