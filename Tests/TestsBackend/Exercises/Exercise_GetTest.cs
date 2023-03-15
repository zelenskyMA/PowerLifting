using FluentAssertions;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Exercises;

public class Exercise_GetTest : BaseTest
{
    private readonly int countInDictionary = 11;

    public Exercise_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Exercise_GetById_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Get($"/exerciseInfo/{Constants.ExType1Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Exercise_GetById_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<Exercise>($"/exerciseInfo/-1");

        //Assert
        response.Should().NotBeNull(); // заглушка
        response.Name.Should().BeNullOrEmpty();
        response.Id.Should().Be(0);
    }

    [Fact]
    public void Exercise_GetById_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<Exercise>($"/exerciseInfo/{Constants.ExType1Id}");

        //Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(Constants.ExType1Id);
        response.Name.Should().NotBeEmpty();
        response.ExerciseSubTypeId.Should().BeGreaterThan(0);
        response.ExerciseTypeId.Should().BeGreaterThan(0);
        response.UserId.Should().Be(0); // справочное упражнение
        response.PlannedExerciseId.Should().Be(0); // нет информации о запланированности через этот эндпоинт
    }

    [Fact]
    public void Exercise_GetList_PresetOnly_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act - Assert
        var response = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        response.Count.Should().Be(countInDictionary);

        response = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        response.Count.Should().Be(0);

        response = Client.Get<List<Exercise>>($"/exerciseInfo/getAdminEditingList");
        response.Count.Should().Be(countInDictionary);
    }

    [Fact]
    public void Exercise_GetList_WithPersonal_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var exName = "testEx1";
        var request = new Exercise() { Name = exName, ExerciseTypeId = 1, ExerciseSubTypeId = Constants.SubTypeId };
        var createResult = Client.Post<bool>($"/exerciseInfo/", request);
        createResult.Should().BeTrue();

        //Act - Assert
        var response = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        response.Count.Should().Be(countInDictionary + 1);

        response = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        response.Count.Should().Be(1);

        response = Client.Get<List<Exercise>>($"/exerciseInfo/getAdminEditingList");
        response.Count.Should().Be(countInDictionary);

        // откат
        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        var personalItem = items.FirstOrDefault(t => t.Name == exName);
        var deleteResponse = Client.Delete<bool>($"/exerciseInfo/{personalItem.Id}");
        deleteResponse.Should().BeTrue();
    }
}
