using FluentAssertions;
using SportAssistant.Application.TrainingPlan.PlanExerciseCommands;
using SportAssistant.Domain.Models.TrainingPlan;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Exercises;

public class Exercise_CreateUpdateTest : BaseTest
{
    private readonly int exTypeId = 1;

    public Exercise_CreateUpdateTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Exercise_Update_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var request = new Exercise() { Name = "test1", ExerciseTypeId = exTypeId, ExerciseSubTypeId = TestConstants.SubTypeId };
        var response = Client.Post($"/exerciseInfo", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Exercise_Update_NoParams_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act - Assert
        var response = Client.Post($"/exerciseInfo/", new Exercise() { Name = string.Empty, ExerciseTypeId = exTypeId, ExerciseSubTypeId = TestConstants.SubTypeId });
        response.ReadErrorMessage().Should().Match("Название обязательно для заполнения");

        response = Client.Post($"/exerciseInfo/", new Exercise() { Name = "sss", ExerciseTypeId = 0, ExerciseSubTypeId = TestConstants.SubTypeId });
        response.ReadErrorMessage().Should().Match("Укажите существующий тип упражнения");

        response = Client.Post($"/exerciseInfo/", new Exercise() { Name = "sss", ExerciseTypeId = exTypeId, ExerciseSubTypeId = 0 });
        response.ReadErrorMessage().Should().Match("Укажите существующий подтип упражнения");
    }

    [Fact]
    public void Exercise_Update_WrongParams_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act - Assert
        var response = Client.Post($"/exerciseInfo/", new Exercise() { Name = "nmk", ExerciseTypeId = 100, ExerciseSubTypeId = TestConstants.SubTypeId });
        response.ReadErrorMessage().Should().Match("Укажите существующий тип упражнения");

        response = Client.Post($"/exerciseInfo/", new Exercise() { Name = "nmk", ExerciseTypeId = exTypeId, ExerciseSubTypeId = 1000 });
        response.ReadErrorMessage().Should().Match("Укажите существующий подтип упражнения");
    }


    [Fact]
    public void Exercise_Create_Duplicate_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var request = new Exercise() { Name = "test1", ExerciseTypeId = exTypeId, ExerciseSubTypeId = TestConstants.SubTypeId };

        //Act
        var createResponse = Client.Post<bool>($"/exerciseInfo/", request); // создаем успешно
        createResponse.Should().BeTrue();

        var response = Client.Post($"/exerciseInfo/", request); // создаем упражнение с тем же названием

        //Assert
        response.ReadErrorMessage().Should().Match("Упражнение с таким названием уже существует*");
    }

    [Fact]
    public void Exercise_Update_DictByUser_Fail() // спортсмен не может редактировать справочные упражнения
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        var request = items[0];
        request.Description = "---";

        //Act
        var response = Client.Post($"/exerciseInfo/", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Базовый справочник упражнений редактируют только администраторы*");
    }

    [Fact]
    public void Exercise_Update_DictByAdmin_Success() // админ может редактировать справочные упражнения
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var desc = "---";
        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        var request = items[1];
        request.Description = desc;

        //Act
        var response = Client.Post<bool>($"/exerciseInfo/", request);

        //Assert
        response.Should().BeTrue();

        items = Client.Get<List<Exercise>>($"/exerciseInfo/getPlanningList");
        items[1].Description.Should().Be(desc);
    }

    [Fact]
    public void Exercise_Update_PersonalByOthers_Fail() // тренеры, админы, юзеры - не могут редактировать чужие персональные упражнения
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var exName = "zzz";
        var request = new Exercise() { Name = exName, ExerciseTypeId = exTypeId, ExerciseSubTypeId = TestConstants.SubTypeId };
        Client.Post<bool>($"/exerciseInfo/", request);

        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        request.Id = items.FirstOrDefault(t => t.Name == exName).Id;
        request.Description = exName;

        //Act - Assert | by admin
        Factory.Actions.AuthorizeAdmin(Client);
        var response = Client.Post($"/exerciseInfo/", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на редактирование данного упражнения*");

        //Act - Assert | by admin
        Factory.Actions.AuthorizeCoach(Client);
        response = Client.Post($"/exerciseInfo/", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на редактирование данного упражнения*");
    }


    [Fact]
    public void Exercise_Create_Personal_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var desc = "test description";
        var exName = "uuu";
        var request = new Exercise() { Name = exName, ExerciseTypeId = exTypeId, ExerciseSubTypeId = TestConstants.SubTypeId, Description = desc };

        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList"); // проверяем, что нет упражнений
        items.FirstOrDefault(t => t.Name == exName).Should().BeNull();

        //Act
        var response = Client.Post<bool>($"/exerciseInfo/", request);

        //Assert
        response.Should().BeTrue();

        items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");
        var item = items.FirstOrDefault(t => t.Name == exName);
        item.Should().NotBeNull();
        item.Id.Should().BeGreaterThan(0);
        item.Name.Should().Be(exName);
        item.Description.Should().Be(desc);
        item.Closed.Should().BeFalse();
        item.ExerciseTypeId.Should().Be(exTypeId);
        item.ExerciseSubTypeId.Should().Be(TestConstants.SubTypeId);
        item.ExerciseTypeName.Should().NotBeNullOrEmpty();
        item.ExerciseSubTypeName.Should().NotBeNullOrEmpty();

        item.UserId.Should().Be(Factory.Data.GetUserId(TestConstants.UserLogin));
        item.PlannedExerciseId.Should().Be(0);
    }

    [Fact]
    public void Exercise_Update_Personal_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var desc = "test description";
        var exName = "ddd";
        var changeData = " sss";
        var request = new Exercise() { Name = exName, ExerciseTypeId = exTypeId, ExerciseSubTypeId = TestConstants.SubTypeId, Description = desc };
        Client.Post<bool>($"/exerciseInfo/", request); // создаем упражнение

        var items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList"); // проверяем, что есть личное упражнение
        var changeItem = items.FirstOrDefault(t => t.Name == exName);
        changeItem.Should().NotBeNull();

        //Act
        request.Id = changeItem.Id;
        request.Name = exName + changeData;
        request.Description = desc + changeData;
        var response = Client.Post<bool>($"/exerciseInfo/", request); // обновляем упражнение

        //Assert
        response.Should().BeTrue();

        items = Client.Get<List<Exercise>>($"/exerciseInfo/getEditingList");

        items.FirstOrDefault(t => t.Name == exName).Should().BeNull(); // упражнение со старым именем исчезло

        var item = items.FirstOrDefault(t => t.Name == exName + changeData);
        item.Id.Should().BeGreaterThan(0);
        item.Name.Should().Be(exName + changeData);
        item.Description.Should().Be(desc + changeData);
        item.Closed.Should().BeFalse();
        item.ExerciseTypeId.Should().Be(exTypeId);
        item.ExerciseSubTypeId.Should().Be(TestConstants.SubTypeId);
        item.ExerciseTypeName.Should().NotBeNullOrEmpty();
        item.ExerciseSubTypeName.Should().NotBeNullOrEmpty();

        item.UserId.Should().Be(Factory.Data.GetUserId(TestConstants.UserLogin));
        item.PlannedExerciseId.Should().Be(0);
    }

}
