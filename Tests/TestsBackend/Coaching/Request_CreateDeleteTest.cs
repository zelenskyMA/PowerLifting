using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class Request_CreateDeleteTest : BaseTest
{
    public Request_CreateDeleteTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Create_Request_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);

        var response = Client.Post($"/trainingRequests/create?coachId={coachId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Create_Request_WrongCoachId_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Post($"/trainingRequests/create?coachId=0");
        response.ReadErrorMessage().Should().Match("Тренер, которому вы подаете заявку, не найден*");
    }

    [Fact]
    public void Create_Request_ToSelf_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);

        var response = Client.Post($"/trainingRequests/create?coachId={coachId}");
        response.ReadErrorMessage().Should().Match("Нельзя подать заявку самому себе*");
    }

    [Fact]
    public void Create_Request_Multiple_Fail()
    {
        // Arrange
        Factory.Actions.AuthorizeUserNoCoach(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);

        var createResult = Client.Post<bool>($"/trainingRequests/create?coachId={coachId}"); // создаем первую заявку
        createResult.Should().BeTrue();

        //Act
        var response = Client.Post($"/trainingRequests/create?coachId={coachId}"); // создаем вторую заявку

        //Assert
        response.ReadErrorMessage().Should().Match("Вы уже подали заявку тренеру*");
        RemoveRequest(); // удаляем первую заявку
    }

    [Fact]
    public void Delete_Request_NotExisting_Success()
    {
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        Factory.Actions.AuthorizeUserNoCoach(Client);

        var removeResult = Client.Post<bool>($"/trainingRequests/remove?userId={userId}");
        removeResult.Should().BeTrue(); // Удаление несуществующей заявки не падает с ошибкой.
    }

    [Fact]
    public void CreateAndDelete_Request_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUserNoCoach(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);

        // Успешно создали
        //Act
        var createResult = Client.Post<bool>($"/trainingRequests/create?coachId={coachId}");
        createResult.Should().BeTrue();

        //Assert
        var request = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest");
        request.Should().NotBeNull();
        request.UserId.Should().Be(userId);
        request.UserName.Should().NotBeEmpty();
        request.CoachId.Should().Be(coachId);
        request.CoachName.Should().NotBeEmpty();
        request.CreationDate.Should().BeBefore(DateTime.Now);


        // Успешно удалили
        //Act
        var removeResult = Client.Post<bool>($"/trainingRequests/remove?userId={userId}");
        removeResult.Should().BeTrue();

        //Assert
        request = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest");
        request.Should().NotBeNull(); // ожидаем заглушку
        request.UserId.Should().Be(0);
        request.CoachId.Should().Be(0);
    }

    /// <summary>
    /// Удаляем запрос к тренеру
    /// </summary>
    private void RemoveRequest()
    {
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        var removeResult = Client.Post<bool>($"/trainingRequests/remove?userId={userId}");
        removeResult.Should().BeTrue();
    }
}
