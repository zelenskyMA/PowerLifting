using FluentAssertions;
using SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace TrainingTemplates;

public class TemplateExercise_CreateTest : BaseTest
{
    public TemplateExercise_CreateTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_TmpltExercise_WrongDayId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TemplateExerciseCreateCommand.Param()
        {
            DayId = 0,
            Exercises = GetExercises()
        };

        //Act
        var response = Client.Post($"/templateExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Не найден день шаблона с Ид*");
    }

    [Fact]
    public void Create_TmpltExercise_NoItems_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        int dayCounter = 4;
        var planDayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[dayCounter].Id;
        var request = new TemplateExerciseCreateCommand.Param()
        {
            DayId = planDayId,
            Exercises = new List<Exercise>()
        };

        //Act
        var response = Client.Post<bool>($"/templateExercise", request);

        //Assert
        response.Should().BeFalse();
    }

    [Fact]
    public void Create_TmpltExercise_ByOthers_Fail()
    {
        //Arrange
        int dayCounter = 4;
        var planDayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[dayCounter].Id;
        var request = new TemplateExerciseCreateCommand.Param() { DayId = planDayId, Exercises = GetExercises() };

        Factory.Actions.AuthorizeCoach(Client); // чужим инфа недоступна
        var testDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}"); // проверяем, что нет упражнений
        testDay.Exercises.Should().BeEmpty();

        Factory.Actions.AuthorizeSecondCoach(Client);

        //Act
        var response = Client.Post($"/templateExercise", request);

        //Assert
        response.ReadErrorMessage().Should().Match("У вас нет права изменять данные в выбранном тренировочном цикле*");
    }
    
    [Fact]
    public void Create_TmpltExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        int dayCounter = 5;
        var planDayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[dayCounter].Id;
        var request = new TemplateExerciseCreateCommand.Param() { DayId = planDayId, Exercises = GetExercises() };

        var testDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}"); // проверяем, что нет упражнений
        testDay.Exercises.Should().BeEmpty();

        //Act
        var response = Client.Post<bool>($"/templateExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(GetExercises().Count);
    }

    [Fact]
    public void CreateAndRemove_TmpltExercise_ByOwner_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        int dayCounter = 1;
        var planDayId = Factory.Data.TemplateSet.Templates[0].TrainingDays[dayCounter].Id;
        var request = new TemplateExerciseCreateCommand.Param()
        {
            DayId = planDayId,
            Exercises = new List<Exercise>() { GetExercises().First() } // берем только одно
        };

        var testDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}"); // проверяем, что есть 2 упражнения
        testDay.Exercises.Count.Should().Be(2);

        //Act
        var response = Client.Post<bool>($"/templateExercise", request);

        //Assert
        response.Should().BeTrue();

        var updatedDay = Client.Get<TemplateDay>($"/templateDay/{planDayId}");
        updatedDay.Exercises.Count.Should().Be(1); // было 2, стало 1.
    }

    private List<Exercise> GetExercises() => Factory.Data.TemplateSet.Templates[0].TrainingDays[0].Exercises.Select(t => t.Exercise).ToList();
}
