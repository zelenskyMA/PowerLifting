using FluentAssertions;
using SportAssistant.Domain.Models.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateDay_GetTest : BaseTest
{
    public TemplateDay_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_TmpltDay_UnAuthorized_Fail()
    {
        //Arrange
        Factory.Actions.UnAuthorize(Client);
        var dayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Id;

        //Act
        var response = Client.Get($"/templateDay/{dayId}");

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_TmpltDay_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Get($"/templateDay/-1");

        //Assert
        string respContentString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        respContentString.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Get_TmpltDay_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeSecondCoach(Client);
        var dayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Id;

        //Act
        var response = Client.Get($"/templateDay/{dayId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_TmpltDay_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var dayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Id;

        //Act
        var response = Client.Get<TemplateDay>($"/templateDay/{dayId}");

        //Assert
        response.Should().NotBeNull();
        VerifyPlanCheck(response, dayId);
    }

    private void VerifyPlanCheck(TemplateDay tmpltDay, int? dayId)
    {
        tmpltDay.Id.Should().Be(dayId);
        tmpltDay.DayNumber.Should().BeGreaterThan(0);
        tmpltDay.Counters.WeightLoadPercentageSum.Should().BeGreaterThan(0);
        tmpltDay.Counters.LiftCounterSum.Should().BeGreaterThan(0);

        tmpltDay.Percentages.Should().NotBeNull();
        tmpltDay.Percentages[0].Id.Should().BeGreaterThan(0);
        tmpltDay.Percentages[0].MaxValue.Should().BeGreaterThan(0);
        tmpltDay.Percentages[0].MinValue.Should().Be(0);
        tmpltDay.Percentages[0].Name.Should().NotBeNullOrEmpty();
        tmpltDay.Counters.ExerciseTypeCounters.Should().NotBeEmpty();

        tmpltDay.Counters.ExerciseTypeCounters[0].Value.Should().BeGreaterThan(0);
        tmpltDay.Counters.ExerciseTypeCounters[0].Name.Should().NotBeNullOrEmpty();

        // Упражнения в тренировочном дне        
        tmpltDay.Exercises.Should().HaveCount(2);
        var dayExercise = tmpltDay.Exercises[0];
        dayExercise.Id.Should().BeGreaterThan(0);
        dayExercise.TemplateDayId.Should().Be(tmpltDay.Id);
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
