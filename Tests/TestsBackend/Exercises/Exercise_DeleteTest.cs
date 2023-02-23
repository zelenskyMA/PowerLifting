using FluentAssertions;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Exercises;

public class Exercise_DeleteTest : BaseTest
{
    public Exercise_DeleteTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Exercise_Delete_WrongId_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Delete($"/exerciseInfo/-1");
        response.ReadErrorMessage().Should().Match("Выбранное упражнение не существует*");

        response = Client.Delete($"/exerciseInfo/1000");
        response.ReadErrorMessage().Should().Match("Выбранное упражнение не существует*");
    }

    [Fact]
    public void Exercise_Delete_DictByUser_Fail() // спортсмен не может удалять справочные упражнения
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");

        //Act
        var response = Client.Delete($"/exerciseInfo/{items[5].Id}");

        //Assert
        response.ReadErrorMessage().Should().Match("Базовый справочник упражнений редактируют только администраторы*");
    }

    [Fact]
    public void Exercise_Delete_DictByAdmin_Success() // админ может удалять справочные упражнения
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        var count = items.Count();

        //Act
        var response = Client.Delete<bool>($"/exerciseInfo/{items[5].Id}");

        //Assert
        response.Should().BeTrue();

        items = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        items.Count.Should().Be(count - 1);
    }

    [Fact]
    public void Exercise_Delete_PersonalByOthers_Fail() // тренеры, админы, юзеры - не могут удалять чужие персональные упражнения
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var exName = "zzz";
        var request = new Exercise() { Name = exName, ExerciseTypeId = 1, ExerciseSubTypeId = Constants.SubTypeId };
        Client.Post<bool>($"/exerciseInfo/", request);

        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        var personalItem = items.FirstOrDefault(t => t.Name == exName);

        //Act - Assert | by admin
        Factory.Actions.AuthorizeAdmin(Client);
        var response = Client.Delete($"/exerciseInfo/{personalItem.Id}");
        response.ReadErrorMessage().Should().Match("У вас нет прав на редактирование данного упражнения*");

        //Act - Assert | by admin
        Factory.Actions.AuthorizeCoach(Client);
        response = Client.Delete($"/exerciseInfo/{personalItem.Id}");
        response.ReadErrorMessage().Should().Match("У вас нет прав на редактирование данного упражнения*");
    }

    [Fact]
    public void Exercise_Delete_Personal_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var exName = "gfjg";
        var request = new Exercise() { Name = exName, ExerciseTypeId = 1, ExerciseSubTypeId = Constants.SubTypeId };
        Client.Post<bool>($"/exerciseInfo/", request);

        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        var personalItem = items.FirstOrDefault(t => t.Name == exName);
        personalItem.Should().NotBeNull();

        //Act
        var response = Client.Delete<bool>($"/exerciseInfo/{personalItem.Id}");

        //Assert
        response.Should().BeTrue();

        var updItems = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        var closedItem = updItems.FirstOrDefault(t => t.Name == exName);
        closedItem.Should().BeNull();
    }
}
