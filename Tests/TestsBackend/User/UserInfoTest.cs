using AutoFixture;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using SportAssistant.Domain.Models.UserData;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace User;

public class UserInfoTest : BaseTest
{
    public UserInfoTest(ServiceTestFixture<Program> factory) : base(factory) {
        Factory.Actions.AuthorizeUser(Client);
    }

    [Fact]
    public void Get_Update_UserInfo_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);

        //Act
        // исходные данные корректно возвращаются
        var info = Client.Get<UserInfo>("/userInfo/get");
        info.Should().NotBeNull();
        info.FirstName.IsNullOrEmpty().Should().BeFalse();
        info.Age.Should().NotBeNull();

        // обновление данных пользователя
        var newInfo = Factory.GetBuilder().Build().Create<UserInfo>();

        var response = Client.Post<bool>("/userInfo/update", newInfo);
        response.Should().BeTrue();

        //Assert - проверяем обновление
        var assertInfo = Client.Get<UserInfo>("/userInfo/get");
        assertInfo.FirstName.Should().NotBe(info.FirstName);
        assertInfo.Age.Should().NotBe(info.Age);
        assertInfo.Should().BeEquivalentTo(newInfo, t => t
            .Excluding(m => m.LegalName)
            .Excluding(m => m.CoachLegalName)
            .Excluding(m => m.RolesInfo));
    }

    [Fact]
    public void Get_Card_Wrong_Request_Fail()
    {
        var response = Client.Get("/userInfo/getCard");
        response.ReadErrorMessage().Should().Match("Пользователь не найден*");

        response = Client.Get("/userInfo/getCard?userId=999");
        response.ReadErrorMessage().Should().Match("Пользователь не найден*");
    }

    [Fact]
    public void Get_Card_No_Rights_Fail()
    {
        //Arrange
        Factory.Actions.AuthorizeUser(Client);
        var userId = Factory.Data.GetUserId(Constants.NoCoachUserLogin);

        //Act
        var response = Client.Get($"/userInfo/getCard?userId={userId}");

        //Assert
        response.ReadErrorMessage().Should().Match("Нет прав для просмотра данной информации*");
    }


    [Fact]
    public void Get_Card_Self_BaseInfo_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var userId = Factory.Data.GetUserId(Constants.AdminLogin);
        var newInfo = Factory.GetBuilder().Build().Create<UserInfo>();

        var response = Client.Post<bool>("/userInfo/update", newInfo);
        response.Should().BeTrue();

        //Act
        var card = Client.Get<UserCard>($"/userInfo/getCard?userId={userId}");

        //Assert обновления
        card.Login.Should().BeEquivalentTo(Constants.AdminLogin);
        card.BaseInfo.Surname.Should().BeEquivalentTo(newInfo.Surname);
    }

    [Fact]
    public void Get_Card_ByAdmin_Blocked_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var blockedUserId = Factory.Data.GetUserId(Constants.BlockedUserLogin);

        //Act
        var card = Client.Get<UserCard>($"/userInfo/getCard?userId={blockedUserId}");

        //Assert
        card.Login.Should().BeEquivalentTo(Constants.BlockedUserLogin);
        card.BlockReason?.BlockerId.Should().Be(Factory.Data.GetUserId(Constants.AdminLogin));
        card.BlockReason.Should().NotBeNull();
    }

    [Fact]
    public void Get_Card_ByCoach_Blocked_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeCoach(Client);
        var blockedUserId = Factory.Data.GetUserId(Constants.BlockedUserLogin);

        //Act
        var card = Client.Get<UserCard>($"/userInfo/getCard?userId={blockedUserId}");

        //Assert
        card.Login.Should().BeEquivalentTo(Constants.BlockedUserLogin);
        card.BlockReason?.BlockerId.Should().Be(Factory.Data.GetUserId(Constants.AdminLogin));
        card.BlockReason.Should().NotBeNull();
    }
}