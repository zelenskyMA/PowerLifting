using FluentAssertions;
using SportAssistant.Domain.Models.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateExercise_GetTest : BaseTest
{
    public TemplateExercise_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_TmpltExercise_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Get($"/templateExercise/{GetEx().Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_TmpltExercise_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Get<TemplateExercise>($"/templateExercise/-1");

        //Assert
        response.Should().NotBeNull(); // заглушка
        response.Id.Should().Be(0);
    }

    [Fact]
    public void Get_TmpltExercise_ByOthers_Fail()
    {
        Factory.Actions.AuthorizeSecondCoach(Client);
        var response = Client.Get($"/templateExercise/{GetEx().Id}");
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_TmpltExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Get<TemplateExercise>($"/templateExercise/{GetEx().Id}");

        //Assert
        response.Should().NotBeNull();
        VerifyExerciseCheck(response, GetEx().Id);
    }


    [Fact]
    public void GetByDay_TmpltExercise_WrongId_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<List<TemplateExercise>>($"/templateExercise/getByDay/-1");
        response.Should().BeEmpty();
    }

    [Fact]
    public void GetByDay_TmpltExercise_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var dayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Id;

        //Act
        var response = Client.Get($"/templateExercise/getByDay/{dayId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }   

    [Fact]
    public void GetByDay_TmpltExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var dayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Id;

        //Act
        var response = Client.Get<List<TemplateExercise>>($"/templateExercise/getByDay/{dayId}");

        //Assert
        response.Should().NotBeNull();
        response.Count().Should().Be(2);
        VerifyExerciseCheck(response[0], response[0].Id);
    }


    private TemplateExercise GetEx() => Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Exercises[0];


    private void VerifyExerciseCheck(TemplateExercise dayExercise, int? exId)
    {
        dayExercise.Id.Should().Be(exId);
        dayExercise.TemplateDayId.Should().BeGreaterThan(0);
        dayExercise.LiftCounter.Should().BeGreaterThan(0);
        dayExercise.WeightLoadPercentage.Should().BeGreaterThan(0);
        dayExercise.Order.Should().BeGreaterThan(0);

        dayExercise.SettingsTemplate.Should().NotBeNull(); // шаблон не назначен, но заглушка
        dayExercise.SettingsTemplate.Id.Should().Be(0);

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
        settings.WeightPercentage.Should().BeGreaterThan(0);
        settings.Iterations.Should().BeGreaterThan(0);
        settings.ExercisePart1.Should().BeGreaterThan(0);
        settings.ExercisePart2.Should().BeGreaterThan(0);
        settings.ExercisePart3.Should().BeGreaterThan(0);
        settings.TemplateExerciseId.Should().BeGreaterThan(0);

        // процентовка поднятия
        settings.Percentage.Should().NotBeNull();
        settings.Percentage.Id.Should().BeGreaterThan(0);
        settings.Percentage.MaxValue.Should().BeGreaterThan(0);
        settings.Percentage.MinValue.Should().BeGreaterThan(0);
        settings.Percentage.Name.Should().NotBeNullOrEmpty();
        settings.Percentage.Description.Should().BeNullOrEmpty();
    }
}
