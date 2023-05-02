using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateExercise_UpdateTest : BaseTest
{
    private readonly string comment = "update comment";
    private readonly int weight = 20;

    public TemplateExercise_UpdateTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Update_TmpltExercise_WrongEx_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TemplateExerciseUpdateCommand.Param() { TemplateExercise = new TemplateExercise() { Id = -1 } };

        //Act
        var response = Client.Put($"/templateExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Не найдено упражнение для обновления*");
    }

    [Fact]
    public void Update_TmpltExercise_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planExId = Factory.Data.TemplateSet.Templates[0].TrainingDays[1].Exercises.First().Id;
        var exercise = Client.Get<TemplateExercise>($"/templateExercise/{planExId}");

        var request = new TemplateExerciseUpdateCommand.Param() { TemplateExercise = exercise };
        request.TemplateExercise.Comments = comment;
        request.TemplateExercise.Settings[0].WeightPercentage = weight;

        Factory.Actions.AuthorizeSecondCoach(Client);

        //Act
        var response = Client.Put($"/templateExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права изменять данные в выбранном тренировочном цикле*");
    }

    [Fact]
    public void Update_TmpltExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planExId = Factory.Data.TemplateSet.Templates[0].TrainingDays[1].Exercises.First().Id;
        var exercise = Client.Get<TemplateExercise>($"/templateExercise/{planExId}");

        var request = new TemplateExerciseUpdateCommand.Param() { TemplateExercise = exercise };
        var oldSetting = request.TemplateExercise.Settings[0];
        request.TemplateExercise.Comments = comment;
        request.TemplateExercise.Settings[0].WeightPercentage = weight;

        //Act
        var response = Client.Put<bool>($"/templateExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedExercise = Client.Get<TemplateExercise>($"/templateExercise/{planExId}");
        updatedExercise.Should().NotBeNull();
        updatedExercise.Comments.Should().Be(comment); // обновилось упражнение
        var newSetting = updatedExercise.Settings.First(t => t.Id == oldSetting.Id);

        newSetting.WeightPercentage.Should().Be(weight); // обновились настройки поднятия
        newSetting.Percentage.MaxValue.Should().BeLessThan(oldSetting.Percentage.MaxValue); // пересчет процентовки
    }

    [Fact]
    public void CreateUpdate_OFP_TmpltExercise_Success()
    {
        //Arrange        
        Factory.Actions.AuthorizeCoach(Client);

        // создаем упражнение для последующего обновления
        int dayCounter = 3;
        var planDayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[dayCounter].Id;
        var exercise = Client.Get<Exercise>($"/exerciseInfo/{TestConstants.ExType3Id}");
        var createRequest = new TemplateExerciseCreateCommand.Param() { DayId = planDayId, Exercises = new List<Exercise>() { exercise } };

        var createResponse = Client.Post<bool>($"/templateExercise", createRequest);
        createResponse.Should().BeTrue();
        var createdDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}");
        createdDay.Exercises.Count.Should().Be(1);

        // готовимся обновлять данные
        var extData = "5 повторов";
        var request = new TemplateExerciseUpdateCommand.Param() { TemplateExercise = createdDay.Exercises[0] };
        request.TemplateExercise.Comments = comment;
        request.TemplateExercise.ExtPlanData = extData;

        //Act
        var response = Client.Put<bool>($"/templateExercise", request);

        //Assert
        response.Should().BeTrue();
        var updatedDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(1);
        updatedDay.Exercises.First().ExtPlanData.Should().Be(extData);

        var settings = updatedDay.Exercises.First().Settings;
        settings.Should().NotBeEmpty();
        settings[0].WeightPercentage.Should().Be(0);
        settings[0].Percentage.Should().NotBeNull();
        settings[0].Percentage.MaxValue.Should().Be(99); // для офп процентовка в разбросе 90-100% всегда и только она.
    }
}
