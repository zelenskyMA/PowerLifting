using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using RazorPagesProject.Tests;
using SportAssistant.Domain.Models.UserData;
using TestFramework;
using TestFramework.TestExtensions;
using Xunit;

namespace TestsBackend.User;

public class UserInfoTest : IClassFixture<ServiceTestFixture<Program>>
{
    private readonly HttpClient _client;
    private readonly ServiceTestFixture<Program> _factory;

    public UserInfoTest(ServiceTestFixture<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        _factory.AuthorizeUser(_client);
    }

    [Fact]
    public void Get_Update_UserInfo_Success()
    {
        //Arrange
        _factory.AuthorizeUser(_client);

        //Act
        // пустые данные корректно возвращаются
        var info = _client.Get<UserInfo>("/userInfo/get");
        info.Should().NotBeNull();
        info.FirstName.IsNullOrEmpty().Should().BeTrue();
        info.Age.Should().BeNull();

        // обновление данных пользователя
        var newInfo = _factory.GetBuilder().Build().Create<UserInfo>();

        var response = _client.Post<bool>("/userInfo/update", newInfo);
        response.Should().BeTrue();

        //Assert - проверяем обновление
        info = _client.Get<UserInfo>("/userInfo/get");
        info.Should().BeEquivalentTo(newInfo, t => t
            .Excluding(m => m.LegalName)
            .Excluding(m => m.CoachLegalName)
            .Excluding(m => m.RolesInfo));
    }

    [Fact]
    public void Get_Card_Wrong_Request_Fail()
    {
        var response = _client.Get("/userInfo/getCard");
        response.ReadErrorMessage().Should().Match("Пользователь не найден*");

        response = _client.Get("/userInfo/getCard?userId=999");
        response.ReadErrorMessage().Should().Match("Пользователь не найден*");
    }

    [Fact]
    public void Get_Card_No_Rights_Fail()
    {
        //Arrange
        _factory.AuthorizeUser(_client);
        var userId = _factory.Users.First(t => t.Email == Constants.User2Login).Id;

        //Act
        var response = _client.Get($"/userInfo/getCard?userId={userId}");

        //Assert
        response.ReadErrorMessage().Should().Match("Нет прав для просмотра данной информации*");
    }


    [Fact]
    public void Get_Card_Self_BaseInfo_Success()
    {
        //Arrange
        _factory.AuthorizeAdmin(_client);
        var userId = _factory.Users.First(t => t.Email == Constants.AdminLogin).Id;
        var newInfo = _factory.GetBuilder().Build().Create<UserInfo>();

        var response = _client.Post<bool>("/userInfo/update", newInfo);
        response.Should().BeTrue();

        //Act
        var card = _client.Get<UserCard>($"/userInfo/getCard?userId={userId}");

        //Assert обновления
        card.Login.Should().BeEquivalentTo(Constants.AdminLogin);
        card.BaseInfo.Surname.Should().BeEquivalentTo(newInfo.Surname);
    }

    [Fact]
    public void Get_Card_ByAdmin_Blocked_Success()
    {
        //Arrange
        _factory.AuthorizeAdmin(_client);
        var blockedUserId = _factory.Users.First(t => t.Email == Constants.BlockedUserLogin).Id;

        //Act
        var card = _client.Get<UserCard>($"/userInfo/getCard?userId={blockedUserId}");

        //Assert
        card.Login.Should().BeEquivalentTo(Constants.BlockedUserLogin);
        card.BlockReason?.BlockerId.Should().Be(_factory.Users.First(t => t.Email == Constants.AdminLogin).Id);
        card.BlockReason.Should().NotBeNull();
    }

    [Fact]
    public void Get_Card_ByCoach_Blocked_Success()
    {
        //Arrange
        _factory.AuthorizeCoach(_client);
        var blockedUserId = _factory.Users.First(t => t.Email == Constants.BlockedUserLogin).Id;

        //Act
        var card = _client.Get<UserCard>($"/userInfo/getCard?userId={blockedUserId}");

        //Assert
        card.Login.Should().BeEquivalentTo(Constants.BlockedUserLogin);
        card.BlockReason?.BlockerId.Should().Be(_factory.Users.First(t => t.Email == Constants.AdminLogin).Id);
        card.BlockReason.Should().NotBeNull();
    }
}