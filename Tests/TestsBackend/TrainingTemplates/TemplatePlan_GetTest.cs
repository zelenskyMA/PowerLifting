using FluentAssertions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplatePlan_GetTest : BaseTest
{
    public TemplatePlan_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_TmpltPlan_UnAuthorized_Fail()
    {
        //Arrange
        Factory.Actions.UnAuthorize(Client);
        var tmpltId = Factory.Data.TemplateSet.Templates[0].Id;

        //Act
        var response = Client.Get($"/templatePlan/{tmpltId}");

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_TmpltPlan_WrongId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Get($"/templatePlan/-1");
        response.ReadErrorMessage().Should().Match("Шаблон не найден*");
    }

    [Fact]
    public void Get_TmpltPlan_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeSecondCoach(Client);
        var tmpltId = Factory.Data.TemplateSet.Templates[0].Id;

        //Act
        var response = Client.Get($"/templatePlan/{tmpltId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет прав на просмотр данной информации*");
    }

    [Fact]
    public void Get_TmpltPlan_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var tmpltId = Factory.Data.TemplateSet.Templates[0].Id;

        //Act
        var response = Client.Get<TemplatePlan>($"/templatePlan/{tmpltId}");

        //Assert
        response.Should().NotBeNull();
        VerifyPlanCheck(response, tmpltId);
    }

    private void VerifyPlanCheck(TemplatePlan template, int? tmpltId)
    {
        // шаблон
        template.Id.Should().Be(tmpltId);
        template.Name.Should().NotBeNullOrEmpty();
        template.Counters.CategoryCountersSum.Should().NotBeEmpty();

        // день в шаблоне
        template.TrainingDays.Should().NotBeEmpty();
        var tmpltDay = template.TrainingDays[0];

        tmpltDay.Id.Should().BeGreaterThan(0);
        tmpltDay.DayNumber.Should().BeGreaterThan(0);
        tmpltDay.Counters.WeightLoadPercentageSum.Should().BeGreaterThan(0);
        tmpltDay.Counters.LiftCounterSum.Should().BeGreaterThan(0);
        tmpltDay.Percentages.Should().BeNull();

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
