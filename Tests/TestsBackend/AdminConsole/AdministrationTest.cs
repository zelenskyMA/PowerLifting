using FluentAssertions;
using SportAssistant.Application.Administration.AdministrationCommands;
using SportAssistant.Domain.Models.UserData;
using System.Net;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace AdminConsole;

public class AdministrationTest : BaseTest
{
    public AdministrationTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Get_UserCard_Unauthorized_Fail()
    {
        var response = Client.Get("/administration/getCard");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Get_UserCard_No_Rights_Fail()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get($"/administration/getCard?login={TestConstants.BlockedUserLogin}");
        response.ReadErrorMessage().Should().Match("Нет прав для просмотра данной информации*");
    }

    [Fact]
    public void Get_UserCard_ByLogin_Success()
    {
        Factory.Actions.AuthorizeAdmin(Client);
        var response = Client.Get<UserCard>($"/administration/getCard?login={TestConstants.BlockedUserLogin}");

        //Assert
        response.Login.Should().BeEquivalentTo(TestConstants.BlockedUserLogin);
        response.BlockReason.Should().NotBeNull();
    }

    [Fact]
    public void Get_UserCard_Manager_ByLogin_Success()
    {
        Factory.Actions.AuthorizeManager(Client);
        var response = Client.Get<UserCard>($"/administration/getCard?login={TestConstants.BlockedUserLogin}");

        //Assert
        response.Login.Should().BeEquivalentTo(TestConstants.BlockedUserLogin);
        response.BaseInfo.Should().BeNull(); // не базовая инфа
        response.BlockReason.Should().BeNull(); // не базовая инфа
    }

    [Fact]
    public void Get_UserCard_OrgOwner_ByLogin_Success()
    {
        Factory.Actions.AuthorizeOrgOwner(Client);
        var response = Client.Get<UserCard>($"/administration/getCard?login={TestConstants.BlockedUserLogin}");

        //Assert
        response.Login.Should().BeEquivalentTo(TestConstants.BlockedUserLogin);
        response.BaseInfo.Should().BeNull(); // не базовая инфа
        response.BlockReason.Should().BeNull(); // не базовая инфа
    }

    [Fact]
    public void Get_UserCard_ById_Success()
    {
        var userId = Factory.Data.GetUserId(TestConstants.BlockedUserLogin);
        Factory.Actions.AuthorizeAdmin(Client);
        var response = Client.Get<UserCard>($"/administration/getCard?userId={userId}");

        //Assert
        response.Login.Should().BeEquivalentTo(TestConstants.BlockedUserLogin);
        response.BlockReason.Should().NotBeNull();
    }

    [Fact]
    public void Apply_Roles_No_Rights_Fail()
    {
        var userId = Factory.Data.GetUserId(TestConstants.UserLogin);
        var adminId = Factory.Data.GetUserId(TestConstants.AdminLogin);

        Factory.Actions.AuthorizeUser(Client);

        //User has no rights to add role
        var request = new ApplyRolesCommand.Param() { UserId = userId, IsAdmin = true };
        var response = Client.Post("/administration/applyRoles", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");

        //User has no rights to remove role
        request = new ApplyRolesCommand.Param() { UserId = adminId, IsAdmin = false };
        response = Client.Post("/administration/applyRoles", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");

        Factory.Actions.AuthorizeCoach(Client);

        //Coach has no rights to add role
        request = new ApplyRolesCommand.Param() { UserId = userId, IsAdmin = true };
        response = Client.Post("/administration/applyRoles", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");

        //Coach has no rights to remove role
        request = new ApplyRolesCommand.Param() { UserId = adminId, IsAdmin = false };
        response = Client.Post("/administration/applyRoles", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Apply_Roles_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var userId = Factory.Data.GetUserId(TestConstants.NoCoachUserLogin);

        // проверяем текущее значение
        var oldInfo = Client.Get<UserCard>($"/administration/getCard?login={TestConstants.NoCoachUserLogin}");
        oldInfo.Login.Should().BeEquivalentTo(TestConstants.NoCoachUserLogin);
        oldInfo.BaseInfo.RolesInfo.IsCoach.Should().BeFalse();

        //Act - обновляем роли
        var request = new ApplyRolesCommand.Param() { UserId = userId, IsCoach = true };
        var response = Client.Post<bool>("/administration/applyRoles", request);
        response.Should().BeTrue();

        //Assert
        var newInfo = Client.Get<UserCard>($"/administration/getCard?login={TestConstants.NoCoachUserLogin}");
        newInfo.Login.Should().BeEquivalentTo(TestConstants.NoCoachUserLogin);
        newInfo.BaseInfo.RolesInfo.IsCoach.Should().BeTrue();

        // откат
        request = new ApplyRolesCommand.Param() { UserId = userId, IsCoach = false };
        response = Client.Post<bool>("/administration/applyRoles", request);
        response.Should().BeTrue();
    }

    [Fact]
    public void Apply_Block_No_Rights_Fail()
    {
        var userId = Factory.Data.GetUserId(TestConstants.NoCoachUserLogin);
        Factory.Actions.AuthorizeUser(Client);

        //User has no rights to block
        var request = new ApplyBlockCommand.Param() { UserId = userId, Status = true, Reason = "test" };
        var response = Client.Post("/administration/applyBlock", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");

        //User has no rights to remove block
        request = new ApplyBlockCommand.Param() { UserId = userId, Status = false, Reason = "test" };
        response = Client.Post("/administration/applyBlock", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");

        Factory.Actions.AuthorizeCoach(Client);

        //Coach has no rights to add block
        request = new ApplyBlockCommand.Param() { UserId = userId, Status = true, Reason = "test" };
        response = Client.Post("/administration/applyBlock", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");

        //Coach has no rights to remove block
        request = new ApplyBlockCommand.Param() { UserId = userId, Status = false, Reason = "test" };
        response = Client.Post("/administration/applyBlock", request);
        response.ReadErrorMessage().Should().Match("У вас нет прав на выполнение данной операции*");
    }

    [Fact]
    public void Apply_Block_Success()
    {
        //Arrange
        Factory.Actions.AuthorizeAdmin(Client);
        var userId = Factory.Data.GetUserId(TestConstants.NoCoachUserLogin);
        var adminId = Factory.Data.GetUserId(TestConstants.AdminLogin);
        var reason = "test";

        // проверяем текущее значение
        var oldInfo = Client.Get<UserCard>($"/administration/getCard?userId={userId}");
        oldInfo.Login.Should().BeEquivalentTo(TestConstants.NoCoachUserLogin);
        oldInfo.BlockReason.Should().BeNull();

        //Act - ставим блокировку
        var request = new ApplyBlockCommand.Param() { UserId = userId, Status = true, Reason = reason };
        var response = Client.Post<bool>("/administration/applyBlock", request);
        response.Should().BeTrue();

        //Assert
        var newInfo = Client.Get<UserCard>($"/administration/getCard?userId={userId}");
        newInfo.Login.Should().BeEquivalentTo(TestConstants.NoCoachUserLogin);
        newInfo.BlockReason.Should().NotBeNull();
        newInfo.BlockReason.BlockerId.Should().Be(adminId);
        newInfo.BlockReason.CreationDate.Should().BeBefore(DateTime.Now);
        newInfo.BlockReason.UserId.Should().Be(userId);
        newInfo.BlockReason.Reason.Should().Be(reason);

        // откат
        request = new ApplyBlockCommand.Param() { UserId = userId, Status = false };
        response = Client.Post<bool>("/administration/applyBlock", request);
        response.Should().BeTrue();
    }
}