using FluentAssertions;
using SportAssistant.Application.Coaching.TrainingGroupCommands;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class Group_CreateDeleteTest : BaseTest
{
    private readonly string name = "test group";
    private readonly string desc = "test description";

    public Group_CreateDeleteTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_Group_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Post($"/trainingGroups/create", NewGroup(Factory.Data.GetUserId(Constants.CoachLogin)));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Create_Group_ByUser_Fail() // при любом переданном ид тренера, бесправный пользователь ничего не создаст.
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Post($"/trainingGroups/create", NewGroup());
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Create_Group_NoName_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Post($"/trainingGroups/create", new TrainingGroup() { });
        response.ReadErrorMessage().Should().Match("Название группы обязательно*");
    }

    [Fact]
    public void Create_Group_DuplicateName_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Post<bool>($"/trainingGroups/create", new TrainingGroup() { Name = name });
        response.Should().BeTrue();

        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == name);
        group.Should().NotBeNull();

        try
        {
            //Act
            var secondResponse = Client.Post($"/trainingGroups/create", new TrainingGroup() { Name = name });

            //Assert
            secondResponse.ReadErrorMessage().Should().Match("Группа с названием * уже существует*");
        }
        catch (Exception) { throw; }
        finally { DeleteGroup(group.Id); }
    }

    [Fact]
    public void Create_Group_Success()
    {
        //Act
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Post<bool>($"/trainingGroups/create", NewGroup());
        response.Should().BeTrue(); // создание идет для пославшего запрос тренера, чей бы логин ни был указан (даже 0)

        //Assert
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == name);
        group.Should().NotBeNull();

        try
        {
            group.CoachId = Factory.Data.GetUserId(Constants.CoachLogin);
            group.Name.Should().Be(name);
            group.Description.Should().Be(desc);
            group.ParticipantsCount.Should().Be(0);
        }
        catch (Exception) { throw; }
        finally { DeleteGroup(group.Id); }
    }

    [Fact]
    public void Delete_Group_NoId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Post($"/trainingGroups/delete", new GroupDeleteCommand.Param() { Id = 0 });
        response.ReadErrorMessage().Should().Match("Группа с Id * не найдена*");
    }

    [Fact]
    public void Delete_Group_HasUsers_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == Constants.GroupName);

        //Act
        var response = Client.Post($"/trainingGroups/delete", new GroupDeleteCommand.Param() { Id = group.Id });

        //Assert
        response.ReadErrorMessage().Should().Match("В удаляемой группе устались участники*");
    }


    [Fact]
    public void Delete_Group_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var createResponse = Client.Post<bool>($"/trainingGroups/create", NewGroup());
        createResponse.Should().BeTrue();
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == name);

        //Act
        DeleteGroup(group.Id);

        //Assert
        var resultGroups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        resultGroups.FirstOrDefault(t => t.Name == name).Should().BeNull();
    }

    /// <summary> Запрос на создание новой группы </summary>
    private TrainingGroup NewGroup(int coachId = 0) => new TrainingGroup() { CoachId = coachId, Name = name, Description = desc };

    /// <summary> Удаление группы </summary>
    private void DeleteGroup(int id)
    {
        var request = new GroupDeleteCommand.Param() { Id = id };
        var response = Client.Post<bool>($"/trainingGroups/delete", request);
        response.Should().BeTrue();
    }
}
