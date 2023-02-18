using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class UserGroup_AssignTest : BaseTest
{
    public UserGroup_AssignTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void User_Assign_UnAuthorized_Fail()
    {
        //Arrange
        var group = GetPredefinedGroup();
        Factory.Actions.UnAuthorize(Client);
        var userId = Factory.Data.GetUserId(Constants.AdminLogin);
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        //Act
        var response = Client.Post("/groupUser/assign", request);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }


    [Fact]
    public void User_Assign_NoGroup_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var userId = Factory.Data.GetUserId(Constants.AdminLogin);
        var request = new TrainingGroupUser() { GroupId = 0, UserId = userId };

        //Act
        var response = Client.Post("/groupUser/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Группа не найдена*");
    }

    [Fact]
    public void User_Assign_NoUser_Fail()
    {
        //Arrange
        var group = GetPredefinedGroup();
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = 0 };

        //Act
        var response = Client.Post("/groupUser/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Пользователь не найден*");
    }

    [Fact]
    public void User_Assign_NotGroupOwner_Success() // нельзя добавлять спортсмена в чужую группу
    {
        //Arrange        
        var group = GetPredefinedGroup();
        Factory.Actions.AuthorizeAdmin(Client); //не владелец группы
        var userId = Factory.Data.GetUserId(Constants.AdminLogin);
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        //Act
        var response = Client.Post($"/groupUser/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Вы не являетесь тренером указанной группы*");
    }

    [Fact]
    public void User_Assign_NewWithoutRequest_Success() // нельзя добавлять в свою группу спортсмена без заявки
    {
        //Arrange        
        var group = GetPredefinedGroup();
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        //Act
        var response = Client.Post($"/groupUser/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Нельзя принять спортсмена в группу без его заявки*");
    }

    [Fact]
    public void User_Assign_TransferBetweenGroups_Success() // перемещаем спортсмена между своими группами
    {
        //Arrange        
        var sourceGroup = GetPredefinedGroup();
        var targetGroup = GetPredefinedGroup(Constants.SecondGroupName);
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new TrainingGroupUser() { GroupId = targetGroup.Id, UserId = userId };

        //  предпроверка - спортсмен в группе
        var oldInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={sourceGroup.Id}");
        oldInfo.Users.FirstOrDefault(t => t.Id == userId).Should().NotBeNull();

        //Act
        var response = Client.Post<bool>($"/groupUser/assign", request);

        //Assert
        response.Should().BeTrue();
        var newInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={targetGroup.Id}"); // в целевой появился
        newInfo.Users.FirstOrDefault(t => t.Id == userId).Should().NotBeNull();

        oldInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={sourceGroup.Id}"); // в сорсовой исчез
        oldInfo.Users.FirstOrDefault(t => t.Id == userId).Should().BeNull();

        //откат - возвращаем пользователя
        request = new TrainingGroupUser() { GroupId = sourceGroup.Id, UserId = userId };
        response = Client.Post<bool>($"/groupUser/assign", request);
        response.Should().BeTrue();
    }

    [Fact]
    public void User_Assign_SameGroup_Success() // корректно ведем себя, когда целевая и сорсовая группа - одна и та же
    {
        //Arrange        
        var sourceGroup = GetPredefinedGroup();
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new TrainingGroupUser() { GroupId = sourceGroup.Id, UserId = userId };

        //  предпроверка - спортсмен в группе
        var oldInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={sourceGroup.Id}");
        oldInfo.Users.FirstOrDefault(t => t.Id == userId).Should().NotBeNull();

        //Act
        var response = Client.Post<bool>($"/groupUser/assign", request);

        //Assert
        response.Should().BeTrue();
        var newInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={sourceGroup.Id}"); // в целевой не исчез
        newInfo.Users.FirstOrDefault(t => t.Id == userId).Should().NotBeNull();
    }

    [Fact]
    public void User_Assign_RequestForAnotherCoach_Success() // новый пользователь с заявкой, но она к другому тренеру.
    {
        //Arrange        
        var group = GetPredefinedGroup(Constants.GroupName);
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        //  предпроверка - спортсмен не в группе
        var oldInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={group.Id}");
        oldInfo.Users.FirstOrDefault(t => t.Id == userId).Should().BeNull();

        // создание заявки
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var coachId = Factory.Data.GetUserId(Constants.SecondCoachLogin); // второй тренер
        var createResult = Client.Post<bool>($"/trainingRequests/create?coachId={coachId}");
        createResult.Should().BeTrue();

        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Post($"/groupUser/assign", request);

        //Assert
        response.ReadErrorMessage().Should().Match("Заявка спортсмена направлена другому тренеру*");

        var newInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={group.Id}"); // спортсмен не добавился
        newInfo.Users.FirstOrDefault(t => t.Id == userId).Should().BeNull();

        //откат - удаляем заявку
        var removeResult = Client.Post<bool>($"/trainingRequests/remove?userId={userId}");
        removeResult.Should().BeTrue();
    }

    [Fact]
    public void User_Assign_UserByRequest_Success()
    {
        //Arrange        
        var group = GetPredefinedGroup(Constants.GroupName);
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        var request = new TrainingGroupUser() { GroupId = group.Id, UserId = userId };

        //  предпроверка - спортсмен не в группе
        var oldInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={group.Id}");
        oldInfo.Users.FirstOrDefault(t => t.Id == userId).Should().BeNull();

        // создание заявки
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);
        var createResult = Client.Post<bool>($"/trainingRequests/create?coachId={coachId}");
        createResult.Should().BeTrue();

        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Post<bool>($"/groupUser/assign", request);

        //Assert
        response.Should().BeTrue();
        var newInfo = Client.Get<TrainingGroupInfo>($"/trainingGroups/get?id={group.Id}"); // добавился успешно
        newInfo.Users.FirstOrDefault(t => t.Id == userId).Should().NotBeNull();

        var requestInfo = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest"); // заявки нет, только заглушка
        requestInfo.Should().NotBeNull();
        requestInfo.Id.Should().Be(0);

        //откат - удаляем спортсмена из группы
        response = Client.Post<bool>($"/groupUser/remove", request);
        response.Should().BeTrue();
    }


    private TrainingGroup GetPredefinedGroup(string name = "")
    {
        name = string.IsNullOrEmpty(name) ? Constants.GroupName : name;

        Factory.Actions.AuthorizeCoach(Client);
        var groups = Client.Get<List<TrainingGroup>>($"/trainingGroups/getList");
        var group = groups.FirstOrDefault(t => t.Name == name);
        group.Should().NotBeNull();

        return group;
    }
}

