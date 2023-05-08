using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class Group_GetTest : BaseTest
{
    public Group_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_GroupById_WrongId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Get($"/trainingGroups/0");
        response.ReadErrorMessage().Should().Match("Группа не найдена*");

    }

    [Fact]
    public void Get_GroupById_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == TestConstants.GroupName);
        group.Should().NotBeNull();

        //Act
        var response = Client.Get<TrainingGroupInfo>($"/trainingGroups/{group.Id}");

        //Assert
        response.Should().NotBeNull();
        response.Group.Name.Should().Be(TestConstants.GroupName);
        response.Group.ParticipantsCount.Should().BeGreaterThan(0);
        response.Users.Count.Should().BeGreaterThan(0);

        var user = response.Users.FirstOrDefault(t => t.Id == Factory.Data.GetUserId(TestConstants.UserLogin));
        user.Should().NotBeNull();
        user.FullName.Should().NotBeNullOrEmpty();
        user.ActivePlansCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Get_GroupList_Single_Success()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == TestConstants.GroupName);
        group.Should().NotBeNull();
    }

    [Fact]
    public void Get_GroupList_None_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == TestConstants.GroupName);
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
        info.GroupName.Should().Be(TestConstants.GroupName);
        info.PlanDay.PlanId.Should().Be(planDays[0].PlanId);
        info.PlanDay.Id.Should().Be(planDays[0].Id);
    }
}
