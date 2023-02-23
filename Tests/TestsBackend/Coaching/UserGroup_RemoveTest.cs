using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class UserGroup_RemoveTest : BaseTest
{
    public UserGroup_RemoveTest(ServiceTestFixture<Program> factory) : base(factory) { }
   
    [Fact]
    public void User_Remove_NoGroup_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var userId = Factory.Data.GetUserId(Constants.AdminLogin);
        var request = new TrainingGroupUser() { GroupId = 0, UserId = userId };

        //Act
        var response = Client.Post("/groupUser/remove", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Группа не найдена*");
    }

    [Fact]
    public void User_Remove_NoUser_Fail()
    {
        //Arrange
        var group = GetPredefinedGroup();
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = 0 };

        //Act
        var response = Client.Post("/groupUser/remove", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Пользователь не найден*");
    }

    [Fact]
    public void User_Remove_NotGroupOwner_Success() // нельзя удалить спортсмена из чужой группы
    {
        //Arrange        
        var group = GetPredefinedGroup();
        Factory.Actions.AuthorizeAdmin(Client); //не владелец группы
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        //Act
        var response = Client.Post($"/groupUser/remove", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Вы не являетесь тренером указанной группы*");
    }

    [Fact]
    public void User_Remove_Success()
    {
        //Arrange        
        var group = GetPredefinedGroup();
        var userId = Factory.Data.GetUserId(Constants.UserLogin); // пользователь с тренером и группой
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        var oldInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/{group.Id}"); //предпроверка - спортсмен в группе
        oldInfo.Users.FirstOrDefault(t => t.Id == userId).Should().NotBeNull();

        //Act
        var response = Client.Post<bool>($"/groupUser/remove", request);

        //Assert
        response.Should().BeTrue();
        var newInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/{group.Id}");
        newInfo.Users.FirstOrDefault(t => t.Id == userId).Should().BeNull();

        // откат
        // 1) создание заявки
        Factory.Actions.AuthorizeUser(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);
        var createResult = Client.Post<bool>($"/trainingRequests/{coachId}");
        createResult.Should().BeTrue();

        // 2) возвращение юзера в группу
        Factory.Actions.AuthorizeCoach(Client);
        var restoreResult = Client.Post<bool>($"/groupUser/assign", request);
        restoreResult.Should().BeTrue();
    }


    private TrainingGroup GetPredefinedGroup()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == Constants.GroupName);
        group.Should().NotBeNull();

        return group;
    }
}
