using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RazorPagesProject.Tests;
using SportAssistant.Domain.Models.UserData;
using TestFramework;
using TestFramework.TestExtensions;
using Xunit;

namespace TestsBackend.User;

public class UserAchivementTest : IClassFixture<ServiceTestFixture<Program>>
{
    private readonly HttpClient _client;
    private readonly ServiceTestFixture<Program> _factory;

    public UserAchivementTest(ServiceTestFixture<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        _factory.AuthorizeUser(_client);
    }

    [Fact]
    public void Get_Achivements_Unauthorized_Fail()
    {
        _factory.UnAuthorize(_client);
        var response = _client.Get("/userAchivement/get");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        _factory.AuthorizeUser(_client);
    }

    [Fact]
    public void Get_Achivements_Success()
    {
        var response = _client.Get<List<UserAchivement>>("/userAchivement/get");
        response.Count.Should().Be(2); // достижение 1)в рывке, 2)в толчке.
    }

    [Fact]
    public void Create_Achivements_Success()
    {
        //Arrange
        var userId = _factory.Users.First(t => t.Email == Constants.UserLogin).Id;
        var request = new List<UserAchivement>() {
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 1, Result = 25 },
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 2, Result = 100 },
        };

        //Act
        var response = _client.Post<bool>("/userAchivement/create", request);
                
        //Assert - положительный ответ и возврат корректных рекордов
        response.Should().BeTrue();

        var achivements = _client.Get<List<UserAchivement>>("/userAchivement/get");
        achivements.Count.Should().Be(2);
        achivements.First(t=> t.ExerciseTypeId == 1).Result.Should().Be(25);
        achivements.First(t => t.ExerciseTypeId == 2).Result.Should().Be(100);
    }

    [Fact]
    public void Get_ByExercise_Self_Fail()
    {
        var response = _client.Get($"/userAchivement/getByExercise?planExerciseId=0&exerciseTypeId=1");
        response.ReadErrorMessage().Should().Match("Упражнение не найдено, нельзя определить рекорд*");
    }


    [Fact]
    public void Get_ByExercise_Self_Success()
    {
        //Arrange
        var userId = _factory.Users.First(t => t.Email == Constants.UserLogin).Id;
        var request = new List<UserAchivement>() {
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 1, Result = 30 },
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 2, Result = 110 },
        };
        _client.Post<bool>("/userAchivement/create", request);

        var typeId = _factory.PlanDay.Exercises[0].Exercise.ExerciseTypeId;
        var requestStr = $"planExerciseId={_factory.PlanDay.Exercises[0].Id}&exerciseTypeId={typeId}";

        //Act
        var response = _client.Get<UserAchivement>($"/userAchivement/getByExercise?{requestStr}");

        //Assert
        response.Should().NotBeNull();
        response.Result.Should().Be(request.First(t=> t.ExerciseTypeId == typeId).Result);
    }
}