using FluentAssertions;
using SportAssistant.Domain.Models.Coaching;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Coaching;

public class Request_GetTest : BaseTest
{
    public Request_GetTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_Request_UnAuthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Get("/trainingRequests/getMyRequest");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_My_Request_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUserNoCoach(Client);

        //Act
        var response = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest");

        //Assert
        response.Should().NotBeNull(); // ожидаем заглушку
        response.Id.Should().Be(0);
    }

    [Fact]
    public void Get_My_Request_Success()
    {
        //Arrange
        CreateRequest();

        try
        {
            //Act
            var response = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest");

            //Assert
            response.Should().NotBeNull();
            response.UserId.Should().Be(Factory.Data.GetUserId(Constants.NoCoachUserLogin));
            response.UserName.Should().NotBeEmpty();
            response.CoachId.Should().Be(Factory.Data.GetUserId(Constants.CoachLogin));
            response.CoachName.Should().NotBeEmpty();
            response.CreationDate.Should().BeBefore(DateTime.Now);
        }
        catch (Exception) { throw; }
        finally { RemoveRequest(); }

    }

    [Fact]
    public void Get_Coaches_Single_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUserNoCoach(Client);

        //Act
        var response = Client.Get<List<CoachInfo>>("/trainingRequests/getCoaches");

        //Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(1);
        response[0].Id.Should().Be(Factory.Data.GetUserId(Constants.CoachLogin));
        response[0].Name.Should().NotBeEmpty();
    }

    [Fact]
    public void Get_Coaches_ExcludeSelf_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Get<List<CoachInfo>>("/trainingRequests/getCoaches");

        //Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(0);
    }

    [Fact]
    public void Get_CoachRequests_None_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);

        //Act
        var response = Client.Get<List<TrainingRequest>>("/trainingRequests/getCoachRequests");

        //Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(0);
    }

    [Fact]
    public void Get_CoachRequests_Single_Success()
    {
        try
        {
            //Arrange
            CreateRequest();
            Factory.Actions.AuthorizeCoach(Client);

            //Act
            var response = Client.Get<List<TrainingRequest>>("/trainingRequests/getCoachRequests");

            //Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(1);

        }
        catch (Exception) { throw; }
        finally { RemoveRequest(); }
    }

    [Fact]
    public void Get_RequestById_Single_Success()
    {
        try
        {
            //Arrange
            CreateRequest();
            var request = Client.Get<TrainingRequest>("/trainingRequests/getMyRequest"); // свою заявку может получить и пользователь
            Factory.Actions.AuthorizeCoach(Client); // получить заявку по Ид может только тренер

            //Act
            var response = Client.Get<TrainingRequest>($"/trainingRequests/get?id={request.Id}");

            //Assert
            response.Should().NotBeNull();
            response.CoachName.Should().BeNullOrEmpty();
            response.UserName.Should().NotBeNullOrEmpty();
            response.CoachId.Should().Be(Factory.Data.GetUserId(Constants.CoachLogin));
            response.UserId.Should().Be(Factory.Data.GetUserId(Constants.NoCoachUserLogin));
            response.UserAge.Should().BeGreaterThan(0);
            response.UserHeight.Should().BeGreaterThan(0);
            response.UserWeight.Should().BeGreaterThan(0);
        }
        catch (Exception) { throw; }
        finally { RemoveRequest(); }
    }

    [Fact]
    public void Get_RequestById_None_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var response = Client.Get($"/trainingRequests/get?id=0");
        response.ReadErrorMessage().Should().Match("У тренера с Ид*нет заявки с Ид*");
    }


    /// <summary>
    /// Создаем запрос к тренеру от NoCoachUserLogin
    /// </summary>
    private void CreateRequest()
    {
        Factory.Actions.AuthorizeUserNoCoach(Client);
        var coachId = Factory.Data.GetUserId(Constants.CoachLogin);
        var createResult = Client.Post<bool>($"/trainingRequests/create?coachId={coachId}");
        createResult.Should().BeTrue();
    }

    /// <summary>
    /// Удаляем запрос к тренеру
    /// </summary>
    private void RemoveRequest()
    {
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);
        Factory.Actions.AuthorizeUserNoCoach(Client);
        var removeResult = Client.Post<bool>($"/trainingRequests/remove?userId={userId}");
        removeResult.Should().BeTrue();
    }
}
