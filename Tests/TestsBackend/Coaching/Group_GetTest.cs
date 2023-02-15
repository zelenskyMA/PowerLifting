using FluentAssertions;
using SportAssistant.Application.Coaching.TrainingGroupCommands;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class Group_GetTest : BaseTest
{
    private readonly string name = "test group";
    private readonly string desc = "test description";

    public Group_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_GroupById_WrongId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Get($"/trainingGroups/get?id=0");
        response.ReadErrorMessage().Should().Match("Группа не найдена*");

    }

    [Fact]
    public void Get_GroupById_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == Constants.GroupName);
        group.Should().NotBeNull();

        //Act
        var response = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={group.Id}");

        //Assert
        response.Should().NotBeNull();
        response.Group.Name.Should().Be(Constants.GroupName);
        response.Group.ParticipantsCount.Should().BeGreaterThan(0);
        response.Users.Count.Should().BeGreaterThan(0);

        var user = response.Users.FirstOrDefault(t=> t.Id == Factory.Data.GetUserId(Constants.UserLogin));
        user.Should().NotBeNull();
        user.FullName.Should().NotBeNullOrEmpty();
        user.ActivePlansCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Get_GroupList_Single_Success()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == Constants.GroupName);
        group.Should().NotBeNull();
    }

    [Fact]
    public void Get_GroupList_None_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == Constants.GroupName);
        group.Should().BeNull();
    }

    [Fact]
    public void Get_WorkoutList_None_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var groups = Client.Get<List<TrainingGroupWorkout>>($"/trainingGroups/getWorkoutList");
        groups.Count.Should().Be(0);
    }

    [Fact]
    public void Get_WorkoutList_Single_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var planDays = Factory.Data.PlanDays;
        
        //Act
        var groupsInfo = Client.Get<List<TrainingGroupWorkout>>($"/trainingGroups/getWorkoutList");

        //Assert
        groupsInfo.Count.Should().Be(1);

        var info = groupsInfo.First();
        info.GroupName.Should().Be(Constants.GroupName);
        info.PlanDay.PlanId.Should().Be(planDays[0].PlanId);
        info.PlanDay.Id.Should().Be(planDays[0].Id);
    }

    /// <summary> Создание группы </summary>
    private int CreateGroup(int coachId = 0)
    {
        Factory.Actions.AuthorizeCoach(Client);
        var request = new TrainingGroup() { CoachId = coachId, Name = name, Description = desc };
        var response = Client.Post<bool>($"/trainingGroups/create", request);
        response.Should().BeTrue();

        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == name);
        group.Should().NotBeNull();

        return group.Id;
    }


    /// <summary> Удаление группы </summary>
    private void DeleteGroup(int id)
    {
        var request = new GroupDeleteCommand.Param() { Id = id };
        var response = Client.Post<bool>($"/trainingGroups/delete", request);
        response.Should().BeTrue();
    }
}
