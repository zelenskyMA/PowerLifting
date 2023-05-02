using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using SportAssistant.Application.UserData.UserCommands;
using SportAssistant.Application.UserData.UserCommands.UserCommands;
using SportAssistant.Domain.Models.UserData.Auth;
using TestFramework;
using TestFramework.TestExtensions;
using TestsBackend;
using Xunit;

namespace Common;

public class AuthTest : BaseTest
{
    private readonly string testLogin = "testLogin1@mail.ru";
    private readonly string testPwd = "pwd12345";

    public AuthTest(ServiceTestFixture<Program> factory) : base(factory) { }

    [Fact]
    public void Login_Fail()
    {
        // wrong login - not registered
        var response = Client.Post("/user/login", new UserLoginCommand.Param() { Login = "1", Password = "2" });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден.*");

        // wrong pwd
        response = Client.Post("/user/login", new UserLoginCommand.Param() { Login = TestConstants.AdminLogin, Password = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль указан не верно.*");

        // blocked user can't enter
        response = Client.Post("/user/login", new UserLoginCommand.Param() { Login = TestConstants.BlockedUserLogin, Password = TestConstants.Password });
        response.ReadErrorMessage().Should().Match("Пользователь заблокирован*");
    }

    [Fact]
    public void Login_Success()
    {
        var response = Client.Post<TokenModel>("/user/login", new UserLoginCommand.Param()
        {
            Login = TestConstants.AdminLogin,
            Password = TestConstants.Password
        });

        //Assert
        response.Should().NotBeNull();
        response.Token.IsNullOrEmpty().Should().BeFalse();
        response.RefreshToken.IsNullOrEmpty().Should().BeFalse();
    }

    [Fact]
    public void Login_WithSpace_Success()
    {
        var response = Client.Post<TokenModel>("/user/login", new UserLoginCommand.Param()
        {
            Login = $" {TestConstants.AdminLogin} ",
            Password = TestConstants.Password
        });

        //Assert
        response.Should().NotBeNull();
        response.Token.IsNullOrEmpty().Should().BeFalse();
        response.RefreshToken.IsNullOrEmpty().Should().BeFalse();
    }

    [Fact]
    public void Registration_Fail()
    {
        // no login
        var response = Client.Post("/user/register", new UserRegisterCommand.Param());
        response.ReadErrorMessage().Should().Match("Логин не указан*");

        // invalid email login
        response = Client.Post("/user/register", new UserRegisterCommand.Param() { Login = "invalidEmail" });
        response.ReadErrorMessage().Should().Match("Формат логина не соответствует*");

        // duplicate login
        response = Client.Post("/user/register", new UserRegisterCommand.Param() { Login = TestConstants.AdminLogin, Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином уже существует*");

        // empty password
        response = Client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin });
        response.ReadErrorMessage().Should().Match("Пароль не указан*");

        // short/invalid password
        response = Client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin, Password = "123" });
        response.ReadErrorMessage().Should().Match("Слишком короткий пароль*");

        // no confirmation password
        response = Client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin, Password = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");

        // wrong confirmation password
        response = Client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin, Password = testPwd, PasswordConfirm = "4" });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");
    }

    [Fact]
    public void Registration_Success()
    {
        var response = Client.Post<TokenModel>("/user/register", new UserRegisterCommand.Param()
        {
            Login = "regSuccessUser@mail.ru",
            Password = testPwd,
            PasswordConfirm = testPwd
        });

        //Assert
        response.Should().NotBeNull();
        response.Token.IsNullOrEmpty().Should().BeFalse();
        response.RefreshToken.IsNullOrEmpty().Should().BeFalse();
    }

    [Fact]
    public void Registration_WithSpaces_Success()
    {
        var response = Client.Post<TokenModel>("/user/register", new UserRegisterCommand.Param()
        {
            Login = " regSpaceUser@mail.ru ",
            Password = testPwd,
            PasswordConfirm = testPwd
        });

        //Assert
        response.Should().NotBeNull();
        response.Token.IsNullOrEmpty().Should().BeFalse();
        response.RefreshToken.IsNullOrEmpty().Should().BeFalse();
    }

    [Fact]
    public void ChangePassword_Fail()
    {
        // no login
        var response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param());
        response.ReadErrorMessage().Should().Match("Логин не указан*");

        // invalid email login
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = "invalidEmail" });
        response.ReadErrorMessage().Should().Match("Формат логина не соответствует*");

        // empty password
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin });
        response.ReadErrorMessage().Should().Match("Пароль не указан*");

        // short/invalid password
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = "123" });
        response.ReadErrorMessage().Should().Match("Слишком короткий пароль*");

        // no confirmation password
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");

        // wrong confirmation password
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = testPwd, PasswordConfirm = "4" });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");

        // user not found
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден*");


        // user old password is wrong
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = TestConstants.UserLogin, OldPassword = "1", Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль указан не верно*");

        // user was blocked - change prohibited
        response = Client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = TestConstants.BlockedUserLogin, OldPassword = TestConstants.Password, Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пользователь заблокирован, обратитесь к администрации*");
    }

    [Fact]
    public void ChangePassword_Spaces_Success()
    {
        var response = Client.Post<bool>("/user/changePassword", new UserChangePasswordCommand.Param()
        {
            Login = $" {TestConstants.UserLogin} ",
            OldPassword = TestConstants.Password,
            Password = testPwd,
            PasswordConfirm = testPwd
        });

        //Assert
        response.Should().BeTrue();

        //Откат изменений
        response = Client.Post<bool>("/user/changePassword", new UserChangePasswordCommand.Param()
        {
            Login = TestConstants.UserLogin,
            OldPassword = testPwd,
            Password = TestConstants.Password,
            PasswordConfirm = TestConstants.Password
        });
        response.Should().BeTrue();
    }    

    [Fact]
    public void ResetPassword_Fail()
    {
        // no login
        var response = Client.Post("/user/resetPassword", new UserResetPasswordCommand.Param());
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден*");

        // invalid login
        response = Client.Post("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = testLogin });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден*");

        // user was blocked - reset prohibited
        response = Client.Post("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = TestConstants.BlockedUserLogin });
        response.ReadErrorMessage().Should().Match("Пользователь заблокирован, обратитесь к администрации*");
    }

    [Fact]
    public void ResetPassword_Success()
    {
        //Arrange
        var resetLogin = "resetSuccessUser@mail.ru";
        Client.Post<TokenModel>("/user/register", new UserRegisterCommand.Param() { Login = resetLogin, Password = testPwd, PasswordConfirm = testPwd });

        //Act
        var response = Client.Post<bool>("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = resetLogin });

        //Assert
        response.Should().BeTrue();
    }

    [Fact]
    public void ResetPassword_WithSpaces_Success()
    {
        //Arrange
        var resetLogin = "resetSpaceUser@mail.ru";
        Client.Post<TokenModel>("/user/register", new UserRegisterCommand.Param() { Login = resetLogin, Password = testPwd, PasswordConfirm = testPwd });

        //Act
        var response = Client.Post<bool>("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = resetLogin });

        //Assert
        response.Should().BeTrue();
    }

    [Fact]
    public void RefreshToken_Fail()
    {
        Factory.Actions.UnAuthorize(Client);
        var response = Client.Get("/user/refreshToken");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void RefreshToken_Success()
    {
        Factory.Actions.AuthorizeUser(Client);
        var response = Client.Get<TokenModel>("/user/refreshToken");

        //Assert
        response.Should().NotBeNull();
        response.Token.IsNullOrEmpty().Should().BeFalse();
        response.RefreshToken.IsNullOrEmpty().Should().BeFalse();
    }
}