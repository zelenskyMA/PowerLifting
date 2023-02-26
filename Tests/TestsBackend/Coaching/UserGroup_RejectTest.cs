using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using SportAssistant.Domain.Models.UserData;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class UserGroup_RejectTest : BaseTest
{
    public UserGroup_RejectTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void User_Reject_NoCoachOrRequest_Success()
    {
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var response = Client.Post<bool>("/groupUser/reject");
        response.Should().BeTrue();
    }

    [Fact]
    public void User_Reject_Request_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);

        var createResult = Client.Post<bool>($"/trainingRequests/{coachId}");
        createResult.Should().BeTrue();

        //Act
        var response = Client.Post<bool>("/groupUser/reject");
        response.Should().BeTrue();

        //Assert
        var request = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest");
        request.Should().NotBeNull(); // ожидаем заглушку
        request.UserId.Should().Be(0);
        request.CoachId.Should().Be(0);
    }

    [Fact]
    public void User_Reject_Coach_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        var info = Client.Get<UserInfo>("/userInfo");
        info.CoachId.Should().Be(Factory.Data.GetUserId(Constants.CoachLogin));

        //Act
        var response = Client.Post<bool>("/groupUser/reject");
        response.Should().BeTrue();

        //Assert
        info = Client.Get<UserInfo>("/userInfo");
        info.CoachId.Should().BeNull();

        // откат. Возвращаем тренера пользователю
        // 1) готовим данные
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new TrainingGroupUser() { GroupId = groups.FirstOrDefault(t => t.Name == Constants.GroupName).Id, UserId = userId };
        // 2) создание заявки
        Factory.Actions.AuthorizeUser(Client);
        var createResult = Client.Post<bool>($"/trainingRequests/{coachId}");
        // 3) назначение тренера
        Factory.Actions.AuthorizeCoach(Client);
        var assignment = Client.Post<bool>($"/groupUser/assign", request);
        assignment.Should().BeTrue();
    }
}
