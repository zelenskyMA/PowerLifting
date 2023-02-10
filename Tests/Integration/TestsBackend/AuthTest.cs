using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using RazorPagesProject.Tests;
using SportAssistant.Application.UserData.UserCommands;
using SportAssistant.Application.UserData.UserCommands.UserCommands;
using SportAssistant.Domain.Models.UserData.Auth;
using TestFramework;
using TestFramework.TestExtensions;
using Xunit;

namespace TestsBackend;

public class AuthTest : IClassFixture<ServiceTestFixture<Program>>
{
    private readonly string testLogin = "testLogin1@mail.ru";
    private readonly string testPwd = "pwd12345";

    private readonly HttpClient _client;
    private readonly ServiceTestFixture<Program> _factory;

    public AuthTest(ServiceTestFixture<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public void Login_Fail()
    {
        // wrong login - not registered
        var response = _client.Post("/user/login", new UserLoginCommand.Param() { Login = "1", Password = "2" });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден.*");

        // wrong pwd
        response = _client.Post("/user/login", new UserLoginCommand.Param() { Login = Constants.AdminLogin, Password = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль указан не верно.*");

        // blocked user can't enter
        response = _client.Post("/user/login", new UserLoginCommand.Param() { Login = Constants.BlockedUserLogin, Password = Constants.Password });
        response.ReadErrorMessage().Should().Match("Пользователь заблокирован*");
    }

    [Fact]
    public void Login_Success()
    {
        var response = _client.Post<TokenModel>("/user/login", new UserLoginCommand.Param()
        {
            Login = Constants.AdminLogin,
            Password = Constants.Password
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
        var response = _client.Post("/user/register", new UserRegisterCommand.Param());
        response.ReadErrorMessage().Should().Match("Логин не указан*");

        // invalid email login
        response = _client.Post("/user/register", new UserRegisterCommand.Param() { Login = "invalidEmail" });
        response.ReadErrorMessage().Should().Match("Формат логина не соответствует*");

        // duplicate login
        response = _client.Post("/user/register", new UserRegisterCommand.Param() { Login = Constants.AdminLogin, Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином уже существует*");

        // empty password
        response = _client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin });
        response.ReadErrorMessage().Should().Match("Пароль не указан*");

        // short/invalid password
        response = _client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin, Password = "123" });
        response.ReadErrorMessage().Should().Match("Слишком короткий пароль*");

        // no confirmation password
        response = _client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin, Password = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");

        // wrong confirmation password
        response = _client.Post("/user/register", new UserRegisterCommand.Param() { Login = testLogin, Password = testPwd, PasswordConfirm = "4" });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");
    }

    [Fact]
    public void Registration_Success()
    {
        var response = _client.Post<TokenModel>("/user/register", new UserRegisterCommand.Param()
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
    public void ChangePassword_Fail()
    {
        // no login
        var response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param());
        response.ReadErrorMessage().Should().Match("Логин не указан*");

        // invalid email login
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = "invalidEmail" });
        response.ReadErrorMessage().Should().Match("Формат логина не соответствует*");

        // empty password
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin });
        response.ReadErrorMessage().Should().Match("Пароль не указан*");

        // short/invalid password
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = "123" });
        response.ReadErrorMessage().Should().Match("Слишком короткий пароль*");

        // no confirmation password
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");

        // wrong confirmation password
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = testPwd, PasswordConfirm = "4" });
        response.ReadErrorMessage().Should().Match("Пароль и подтверждение пароля не совпадают*");

        // user not found
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = testLogin, Password = testPwd, PasswordConfirm = testPwd });
        var ss = response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден*");


        // user old password is wrong
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = Constants.UserLogin, OldPassword = "1", Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пароль указан не верно*");

        // user was blocked - change prohibited
        response = _client.Post("/user/changePassword", new UserChangePasswordCommand.Param() { Login = Constants.BlockedUserLogin, OldPassword = Constants.Password, Password = testPwd, PasswordConfirm = testPwd });
        response.ReadErrorMessage().Should().Match("Пользователь заблокирован, обратитесь к администрации*");
    }

    [Fact]
    public void ChangePassword_Success()
    {
        var response = _client.Post<bool>("/user/changePassword", new UserChangePasswordCommand.Param()
        {
            Login = Constants.UserLogin,
            OldPassword = Constants.Password,
            Password = testPwd,
            PasswordConfirm = testPwd
        });

        //Assert
        response.Should().BeTrue();

        //Откат изменений
        response = _client.Post<bool>("/user/changePassword", new UserChangePasswordCommand.Param()
        {
            Login = Constants.UserLogin,
            OldPassword = testPwd,
            Password = Constants.Password,
            PasswordConfirm = Constants.Password
        });
        response.Should().BeTrue();
    }

    [Fact]
    public void ResetPassword_Fail()
    {
        // no login
        var response = _client.Post("/user/resetPassword", new UserResetPasswordCommand.Param());
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден*");

        // invalid login
        response = _client.Post("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = testLogin });
        response.ReadErrorMessage().Should().Match("Пользователь с указанным логином не найден*");

        // user was blocked - reset prohibited
        response = _client.Post("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = Constants.BlockedUserLogin });
        response.ReadErrorMessage().Should().Match("Пользователь заблокирован, обратитесь к администрации*");
    }

    [Fact]
    public void ResetPassword_Success()
    {
        //Arrange
        var resetLogin = "resetSuccessUser@mail.ru";
        _client.Post<TokenModel>("/user/register", new UserRegisterCommand.Param() { Login = resetLogin, Password = testPwd, PasswordConfirm = testPwd });

        //Act
        var response = _client.Post<bool>("/user/resetPassword", new UserResetPasswordCommand.Param() { Login = resetLogin });

        //Assert
        response.Should().BeTrue();
    }

    [Fact]
    public void RefreshToken_Fail()
    {
        _factory.UnAuthorize(_client);
        var response = _client.Get("/user/refreshToken");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void RefreshToken_Success()
    {
        _factory.AuthorizeUser(_client);
        var response = _client.Get<TokenModel>("/user/refreshToken");

        //Assert
        response.Should().NotBeNull();
        response.Token.IsNullOrEmpty().Should().BeFalse();
        response.RefreshToken.IsNullOrEmpty().Should().BeFalse();
    }
}