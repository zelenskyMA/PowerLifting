using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class Group_UpdateTest : BaseTest
{
    private readonly string name = "test group";
    private readonly string desc = "test description";
    private readonly string newName = "changed name";
    private readonly string newDesc = "changed description";

    public Group_UpdateTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Update_Group_WrongId_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TrainingGroup() { Id = 0, Name = newName, Description = newDesc };

        //Act - Assert
        var response = Client.Put($"/trainingGroups", request);
        response.ReadErrorMessage().Should().Match("Группа не существует*");
    }

    [Fact]
    public void Update_Group_ExistDuplicate_Fail() // меняем название на существующее
    {
        //Arrange
        int groupId = CreateGroup();
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TrainingGroup() { Id = groupId, Name = TestConstants.GroupName, Description = newDesc };

        //Act
        var response = Client.Put($"/trainingGroups", request);

        //Assert
        try
        {
            response.ReadErrorMessage().Should().Match("Группа с названием * уже существует*");
        }
        catch (Exception) { throw; }
        finally { DeleteGroup(groupId); }
    }

    [Fact]
    public void Update_Group_NotMine_Fail() // обновление чужой группы
    {
        //Arrange
        int groupId = CreateGroup();
        Factory.Actions.AuthorizeAdmin(Client); // Нельзя даже админу. Инфу группы видит только автор.
        var request = new TrainingGroup() { Id = groupId, Name = newName, Description = newDesc };

        //Act
        var response = Client.Put($"/trainingGroups", request);

        //Assert
        try
        {
            response.ReadErrorMessage().Should().Match("Нельзя редактировать чужую группу*");
        }
        catch (Exception) { throw; }
        finally { DeleteGroup(groupId); }
    }

    [Fact]
    public void Update_Group_Success()
    {
        //Arrange
        int groupId = CreateGroup();
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TrainingGroup() { Id = groupId, Name = newName, Description = newDesc };

        //Act
        var response = Client.Put<bool>($"/trainingGroups", request);

        //Assert
        try
        {
            response.Should().BeTrue();
            var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
            var group = groups.FirstOrDefault(t => t.Name == name);
            group.Should().BeNull();
            group = groups.FirstOrDefault(t => t.Name == newName);
            group.Should().NotBeNull();
            group.CoachId = Factory.Data.GetUserId(TestConstants.CoachLogin);
            group.Name.Should().Be(newName);
            group.Description.Should().Be(newDesc);
            group.ParticipantsCount.Should().Be(0);
        }
        catch (Exception) { throw; }
        finally { DeleteGroup(groupId); }
    }


    /// <summary> Создание группы </summary>
    private int CreateGroup(int coachId = 0)
    {
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TrainingGroup() { CoachId = coachId, Name = name, Description = desc };
        var response = Client.Post<bool>($"/trainingGroups", request);
        response.Should().BeTrue();

        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == name);
        group.Should().NotBeNull();

        return group.Id;
    }


    /// <summary> Удаление группы </summary>
    private void DeleteGroup(int id)
    {
        var response = Client.Delete<bool>($"/trainingGroups/{id}");
        response.Should().BeTrue();
    }
}
