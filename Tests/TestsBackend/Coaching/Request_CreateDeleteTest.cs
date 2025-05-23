﻿using FluentAssertions;
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
        var coachId = Factory.Data.GetUserId(TestConstants.CoachLogin);

        var response = Client.Post($"/trainingRequests/{coachId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Create_Request_WrongCoachId_Fail()
    {
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var response = Client.Post($"/trainingRequests/0");
        response.ReadErrorMessage().Should().Match("Тренер, которому вы подаете заявку, не найден*");
    }

    [Fact]
    public void Create_Request_ToSelf_Fail()
    {
        Factory.Actions.AuthorizeCoach(Client);
        var coachId = Factory.Data.GetUserId(TestConstants.CoachLogin);

        var response = Client.Post($"/trainingRequests/{coachId}");
        response.ReadErrorMessage().Should().Match("Нельзя подать заявку самому себе*");
    }

    [Fact]
    public void Create_Request_Multiple_Fail()
    {
        // Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var coachId = Factory.Data.GetUserId(TestConstants.CoachLogin);

        var createResult = Client.Post<bool>($"/trainingRequests/{coachId}"); // создаем первую заявку
        createResult.Should().BeTrue();

        //Act
        var response = Client.Post($"/trainingRequests/{coachId}"); // создаем вторую заявку

        //Assert
        response.ReadErrorMessage().Should().Match("Вы уже подали заявку тренеру*");
        RemoveRequest(); // удаляем первую заявку
    }

    [Fact]
    public void Create_Request_AlreadyHasCoach_Fail()
    {
        // Arrange
        Factory.Actions.AuthorizeUser(Client);
        var coachId = Factory.Data.GetUserId(TestConstants.CoachLogin);

        //Act
        var response = Client.Post($"/trainingRequests/{coachId}");

        //Assert
        response.ReadErrorMessage().Should().Match("У вас уже есть тренер. Он может быть только один*");
    }

    [Fact]
    public void Delete_Request_NotExisting_Success()
    {
        var userId = Factory.Data.GetUserId(TestConstants.NoCoachUserLogin);
        Factory.Actions.AuthorizeNoCoachUser(Client);

        var removeResult = Client.Delete<bool>($"/trainingRequests/{userId}");
        removeResult.Should().BeTrue(); // Удаление несуществующей заявки не падает с ошибкой.
    }

    [Fact]
    public void CreateAndDelete_Request_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeNoCoachUser(Client);
        var coachId = Factory.Data.GetUserId(TestConstants.CoachLogin);
        var userId = Factory.Data.GetUserId(TestConstants.NoCoachUserLogin);

        // Успешно создали
        //Act
        var createResult = Client.Post<bool>($"/trainingRequests/{coachId}");
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
        var removeResult = Client.Delete<bool>($"/trainingRequests/{userId}");
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
        var userId = Factory.Data.GetUserId(TestConstants.NoCoachUserLogin);
        var removeResult = Client.Delete<bool>($"/trainingRequests/{userId}");
        removeResult.Should().BeTrue();
    }
}
