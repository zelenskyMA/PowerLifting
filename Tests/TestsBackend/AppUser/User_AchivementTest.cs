using AutoFixture;
using FluentAssertions;
using SportAssistant.Domain.Models.UserData;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace AppUser;

public class User_AchivementTest : BaseTest
{
    public User_AchivementTest(ServiceTestFixture<Program> factory) : base(factory)
    {
        Factory.Actions.AuthorizeUser(Client);
    }

    [Fact]
    public void Get_Achivements_Unauthorized_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Get("/userAchivement/get");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        Factory.Actions.AuthorizeUser(Client);
    }

    [Fact]
    public void Get_Achivements_Success()
    {
        var response = Client.Get<List<UserAchivement>>("/userAchivement/get");
        response.Count.Should().Be(2); // достижение 1)в рывке, 2)в толчке.
    }

    [Fact]
    public void Create_Achivements_Success()
    {
        //Arrange
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new List<UserAchivement>() {
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 1, Result = 25 },
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 2, Result = 100 },
        };

        //Act
        var response = Client.Post<bool>("/userAchivement/create", request);
                
        //Assert - положительный ответ и возврат корректных рекордов
        response.Should().BeTrue();

        var achivements = Client.Get<List<UserAchivement>>("/userAchivement/get");
        achivements.Count.Should().Be(2);
        achivements.First(t=> t.ExerciseTypeId == 1).Result.Should().Be(25);
        achivements.First(t => t.ExerciseTypeId == 2).Result.Should().Be(100);
    }

    [Fact]
    public void Get_AchivementsByExercise_Owner_Fail()
    {
        var response = Client.Get($"/userAchivement/getByExercise?planExerciseId=0&exerciseTypeId=1");
        response.ReadErrorMessage().Should().Match("Упражнение не найдено, нельзя определить рекорд*");
    }


    [Fact]
    public void Get_AchivementsByExercise_Owner_Success()
    {
        //Arrange
        var userId = Factory.Data.GetUserId(Constants.UserLogin);
        var request = new List<UserAchivement>() {
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 1, Result = 30 },
            new UserAchivement() { UserId = userId, CreationDate = DateTime.Now, ExerciseTypeId = 2, Result = 110 },
        };
        Client.Post<bool>("/userAchivement/create", request);

        var typeId = Factory.Data.PlanDays[0].Exercises[0].Exercise.ExerciseTypeId;
        var requestStr = $"planExerciseId={Factory.Data.PlanDays[0].Exercises[0].Id}&exerciseTypeId={typeId}";

        //Act
        var response = Client.Get<UserAchivement>($"/userAchivement/getByExercise?{requestStr}");

        //Assert
        response.Should().NotBeNull();
        response.Result.Should().Be(request.First(t=> t.ExerciseTypeId == typeId).Result);
    }
}