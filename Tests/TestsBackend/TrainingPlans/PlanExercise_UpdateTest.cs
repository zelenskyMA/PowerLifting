using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Domain.Models.Basic;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingPlans;

public class PlanExercise_UpdateTest : BaseTest
{
    private readonly string comment = "update comment";
    private readonly int weight = 5000;

    public PlanExercise_UpdateTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Update_PlanExercise_WrongEx_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = new PlanExercise() { Id = -1 } };

        //Act
        var response = Client.Put($"/planExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Не найдено упражнение для обновления*");
    }

    [Fact]
    public void Update_PlanExercise_EcxeedSett_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planExId = Factory.Data.PlanDays[1].Exercises.First().Id;
        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = exercise };

        var settings = Client.Get<AppSettings>("/appSettings");
        var item = request.PlanExercise.Settings.First();
        item.Id = 0;
        for (int i = 0; i <= settings.MaxLiftItems; i++)
        {
            request.PlanExercise.Settings.Add(item); // накидываем поднятий больше лимита
        }

        //Act
        var response = Client.Put($"/planExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Лимит поднятий в упражнении превышен*");
    }

    [Fact]
    public void Update_PlanExercise_WithoutRecord_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planExId = Factory.Data.PlanDays[1].Exercises.First().Id;
        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = exercise };
        request.PlanExercise.Comments = comment;

        //Act
        var response = Client.Put($"/planExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Рекорд спортсмена не указан. Нельзя запланировать тренировку*");
    }

    [Fact]
    public void Update_PlanExercise_ByOthers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planExId = Factory.Data.PlanDays[1].Exercises.First().Id;
        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = exercise };
        request.PlanExercise.Comments = comment;
        request.PlanExercise.Settings[0].Weight = weight;

        Factory.Actions.AuthorizeAdmin(Client);

        //Act
        var response = Client.Put($"/planExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права планировать тренировки данного пользователя*");
    }

    [Fact]
    public void Update_PlanExercise_ByCoach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planExId = Factory.Data.PlanDays[0].Exercises[1].Id;
        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = exercise };
        request.PlanExercise.Comments = comment;
        var oldSetting = request.PlanExercise.Settings[0];
        request.PlanExercise.Settings[0].Weight = weight;

        //Act
        var response = Client.Put<bool>($"/planExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedExercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");
        updatedExercise.Should().NotBeNull();
        updatedExercise.Comments.Should().Be(comment); // обновилось упражнение
        var newSetting = updatedExercise.Settings.First(t => t.Id == oldSetting.Id);

        newSetting.Weight.Should().Be(weight); // обновились настройки поднятия
        newSetting.Percentage.MaxValue.Should().BeGreaterThan(oldSetting.Percentage.MaxValue); // пересчет процентовки
    }

    [Fact]
    public void Update_PlanExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var planExId = Factory.Data.PlanDays[1].Exercises[1].Id;
        var exercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");

        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = exercise };
        request.PlanExercise.Comments = comment;
        var oldSetting = request.PlanExercise.Settings[0];
        request.PlanExercise.Settings[0].Weight = weight;

        //Act
        var response = Client.Put<bool>($"/planExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedExercise = Client.Get<PlanExercise>($"/planExercise/{planExId}");
        updatedExercise.Should().NotBeNull();
        updatedExercise.Comments.Should().Be(comment); // обновилось упражнение
        var newSetting = updatedExercise.Settings.First(t => t.Id == oldSetting.Id);

        newSetting.Weight.Should().Be(weight); // обновились настройки поднятия
        newSetting.Percentage.MaxValue.Should().BeGreaterThan(oldSetting.Percentage.MaxValue); // пересчет процентовки
    }

    [Fact]
    public void CreateUpdate_OFP_PlanExercise_Success()
    {
        //Arrange        
        Factory.Actions.AuthorizeUser(Client);

        // создаем упражнение для последующего обновления
        int dayCounter = 3;
        var planDayId = Factory.Data.PlanDays[dayCounter].Id;
        var exercise = Client.Get<Exercise>($"/exerciseInfo/{TestConstants.ExType3Id}");
        var createRequest = new PlanExerciseCreateCommand.Param() { DayId = planDayId, Exercises = new List<Exercise>() { exercise } };

        var createResponse = Client.Post<bool>($"/planExercise", createRequest);
        createResponse.Should().BeTrue();
        var createdDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        createdDay.Exercises.Count.Should().Be(1);

        // готовимся обновлять данные
        var extData = "5 повторов";
        var request = new PlanExerciseUpdateCommand.Param() { PlanExercise = createdDay.Exercises[0] };
        request.PlanExercise.Comments = comment;
        request.PlanExercise.ExtPlanData = extData;

        //Act
        var response = Client.Put<bool>($"/planExercise", request);

        //Assert
        response.Should().BeTrue();
        var updatedDay = Client.Get<PlanDay>($"/planDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(1);
        updatedDay.Exercises.First().ExtPlanData.Should().Be(extData);

        var settings = updatedDay.Exercises.First().Settings;
        settings.Should().NotBeEmpty();
        settings[0].Weight.Should().Be(0);
        settings[0].Completed.Should().BeFalse();
        settings[0].Percentage.Should().NotBeNull();
        settings[0].Percentage.MaxValue.Should().Be(99); // для офп процентовка в разбросе 90-100% всегда и только она.
    }
}
